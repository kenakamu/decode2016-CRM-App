using Newtonsoft.Json;
using System;

namespace QuickAppointment.Model
{
    /// <summary>
    /// 予定の参加者
    /// JSON プロパティとクラスのプロパティが異なる場合は、JsonProperty 属性を指定
    /// </summary>
    public class Contact
    {
        public string ContactId { get; set; }
        public string FullName { get; set; }
        [JsonProperty("telephone1")]
        public string Telephone { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonProperty("emailaddress1")]
        public string EmailAddress { get; set; }
        public string EntityImage { get; set; }
    }
}
