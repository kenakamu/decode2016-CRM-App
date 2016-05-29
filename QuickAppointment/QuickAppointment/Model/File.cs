using Newtonsoft.Json;

namespace QuickAppointment.Model
{
    /// <summary>
    /// 予定に関連する営業案件に紐づくファイル
    /// </summary>
    public class File
    {
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public bool InEdit { get; set; }
        public int Size { get; set; }
        [JsonProperty("odata.editLink")]
        public string EditLink { get; set; }
    }
}
