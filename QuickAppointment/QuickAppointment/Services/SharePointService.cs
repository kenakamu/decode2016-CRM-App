using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickAppointment.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuickAppointment.Services
{
    public class SharePointService
    {
        // SharePoint Online API のアドレス
        static string apiBase = SettingsService.spoApiBase;

        /// <summary>
        /// ファイルの情報を取得
        /// </summary>
        /// <param name="foldername">ドキュメント保存先</param>
        /// <returns></returns>
        public async Task<List<File>> GetContents(string foldername)
        {
            List<File> files = new List<File>();
            using (HttpClient client = await GetClient())
            {
                HttpResponseMessage res = await client.GetAsync($"getfolderbyserverrelativeurl('/opportunity/{foldername}')/files");
                JToken results = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result) as JToken;

                foreach (var result in results["value"])
                {
                    File file = JsonConvert.DeserializeObject<File>(result.ToString());
                    files.Add(file);
                }

                return files;
            }
        }

        /// <summary>
        /// ファイルの実データ取得
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public async Task<byte[]> GetContent(string relativeUrl)
        {
            using (HttpClient client = await GetClient(true))
            {
                try
                {
                    return await client.GetByteArrayAsync($"getfilebyserverrelativeurl('{relativeUrl}')/$value");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// ファイルの保存
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveContent(string relativeUrl, Stream data)
        {
            using (HttpClient client = await GetClient(true))
            {
                try
                {
                    var res = await client.PutAsync($"getfilebyserverrelativeurl('{relativeUrl}')/$value", new StreamContent(data));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// HttpClient の作成
        /// </summary>
        /// <param name="IsContentDownload">ファイルのダウンロードの場合 true</param>
        /// <returns></returns>
        private async Task<HttpClient> GetClient(bool IsContentDownload = false)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);

            // アクセストークンの設定
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await ADALService.GetAccessToken(SettingsService.spoResource));
            if (!IsContentDownload)
            {
                client.DefaultRequestHeaders.Add(
                    "Accept", "application/json");
            }
            return client;
        }
    }
}
