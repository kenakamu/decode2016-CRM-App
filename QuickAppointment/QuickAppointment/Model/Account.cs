using Newtonsoft.Json;

namespace QuickAppointment.Model
{
    /// <summary>
    /// 予定に関連する営業案件の対象である取引先企業
    /// </summary>
    public class Account
    {
        [JsonProperty("_parentaccountid_value@OData.Community.Display.V1.FormattedValue")]
        public string AccountName { get; set; }
        [JsonProperty("_parentaccountid_value")]
        public string AccountId { get; set; }
    }
}
