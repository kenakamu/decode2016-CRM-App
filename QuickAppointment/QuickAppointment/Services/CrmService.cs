using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickAppointment.DependencyService;
using QuickAppointment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuickAppointment.Services
{
    public class CrmService
    {
        string currentUserId;
        // Dynamics CRM Web API のアドレス
        static string apiBase = SettingsService.crmApiBase;
        SharePointService spservice = new SharePointService();

        /// <summary>
        /// ログインユーザー情報の取得
        /// </summary>
        /// <returns></returns>
        public async Task GetUserId()
        {
            using (HttpClient client = await GetClient())
            {
                try
                {
                    // WhoAmI 関数で UserId を取得
                    HttpResponseMessage res = await client.GetAsync(apiBase + "WhoAmI");
                    JToken result = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result) as JToken;
                    currentUserId = result["UserId"].ToString();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 本日の予定の取得
        /// </summary>
        /// <returns>予定の一覧</returns>
        public async Task<List<Appointment>> GetAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (HttpClient client = await GetClient())
            {
                // ビュー ID を指定して予定を取得
                HttpResponseMessage res = await client.GetAsync(apiBase + "appointments?savedQuery=3E999D0A-7414-E611-80E0-C4346BC53028");
                JToken results = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result) as JToken;

                foreach (var result in results["value"])
                {
                    Appointment appointment = JsonConvert.DeserializeObject<Appointment>(result.ToString());
                    appointment.Attendees = new List<Contact>();
                    appointment.Files = new List<File>();

                    // 予定を関連する参加者の ID を取得
                    // ParticipationTypeMask 5: 必須参加者
                    // ParticipationTypeMask 6: 任意参加者
                    HttpResponseMessage apres = await client.GetAsync(apiBase + $"activityparties?$select=_partyid_value&$filter=(_activityid_value eq {appointment.ActivityId}) and (participationtypemask eq 5 or participationtypemask eq 6)");
                    JToken apresults = JsonConvert.DeserializeObject(apres.Content.ReadAsStringAsync().Result) as JToken;

                    // 予定の参加者の詳細取得
                    foreach (var activity_party in apresults["value"])
                    {
                        // 参加者に自身が含まれる場合はスキップ
                        if (activity_party["_partyid_value"].ToString() == currentUserId.ToString())
                            continue;

                        HttpResponseMessage contactRes = await client.GetAsync(apiBase + $"contacts({activity_party["_partyid_value"]})?$select=fullname,telephone1,emailaddress1,entityimage");
                        Contact contact = JsonConvert.DeserializeObject<Contact>(contactRes.Content.ReadAsStringAsync().Result);

                        appointment.Attendees.Add(contact);
                    }

                    // 資料の取得 appointment.RegardingObjectId は予定に関連する営業案件の ID
                    List<File> files = await GetDocuments(appointment.RegardingObjectId);

                    foreach (var file in files)
                    {
                        appointment.Files.Add(file);
                    }

                    // 予定に関連する取引先企業の取得
                    Account account = await GetAccount(appointment.RegardingObjectId);
                    appointment.Account = account;

                    appointments.Add(appointment);
                }
            }

            return appointments;
        }


        /// <summary>
        /// 会議資料の取得
        /// </summary>
        /// <param name="opportunityId">予定に関連する営業案件の ID</param>
        /// <returns>資料の一覧</returns>
        public async Task<List<File>> GetDocuments(string opportunityId)
        {
            using (HttpClient client = await GetClient())
            {
                // 会議用資料の場所を取得:Dynaimcs CRM と SharePoint Online は連携しており、資料の場所は
                // SharePointDocumentLocation で管理
                HttpResponseMessage res = await client.GetAsync(apiBase + $"sharepointdocumentlocations?$select=relativeurl&$filter=_regardingobjectid_value eq {opportunityId}");
                JToken documentLocation = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result) as JToken;

                if (documentLocation["value"].Count() != 0)
                {
                    // SharePoint Online よりファイル情報の取得
                    List<File> files = await spservice.GetContents(documentLocation["value"][0]["relativeurl"].ToString());
                    return files;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 取引先企業の取得
        /// </summary>
        /// <param name="opportunityId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccount(string opportunityId)
        {
            using (HttpClient client = await GetClient())
            {
                HttpResponseMessage res = await client.GetAsync(apiBase + $"opportunities({opportunityId})?$select=_parentaccountid_value");
                Account account = JsonConvert.DeserializeObject<Account>(res.Content.ReadAsStringAsync().Result);

                return account;
            }
        }

        /// <summary>
        /// 日報の保存
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public async Task SaveAppointment(Appointment appointment)
        {
            // ファイルに変更があった場合、ファイルの保存
            foreach (File file in appointment.Files.Where(x => x.InEdit == true))
            {
                var filedata = await Xamarin.Forms.DependencyService.Get<IFileService>().OpenFile(file.Name);
                await spservice.SaveContent(file.ServerRelativeUrl, filedata);
            }

            using (HttpClient client = await GetClient(true))
            {
                // Put メソッドで特定の列だけを保存
                await client.PutAsync(apiBase + $"appointments({appointment.ActivityId})/new_meetingmemo",
                        new StringContent($"{{'value':'{appointment.Memo}'}}", System.Text.Encoding.UTF8, "application/json"));
            }
        }

        /// <summary>
        /// HttpClient の作成
        /// </summary>
        /// <param name="isUpdate">更新で利用する場合 true　</param>
        /// <returns></returns>
        private async Task<HttpClient> GetClient(bool isUpdate = false)
        {
            HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            
            // アクセストークンの設定
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await ADALService.GetAccessToken(SettingsService.crmResource));

            // 更新ではない場合、FormattedValue も取得
            if (!isUpdate)
            {
                // データは JSON で取得
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                // FormattedValue を取得するため Prefer ヘッダーを指定
                client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");
            }

            return client;
        }
    }
}
