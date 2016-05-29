using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuickAppointment.Model
{
    /// <summary>
    /// 予定
    /// JSON プロパティとクラスのプロパティが異なる場合は、JsonProperty 属性を指定
    /// </summary>
    public　class Appointment
    {
        public string odataetag { get; set; }
        public string Subject { get; set; }
        [JsonProperty("scheduledstart@OData.Community.Display.V1.FormattedValue")]
        public string ScheduledStart { get; set; }
        [JsonProperty("scheduledend@OData.Community.Display.V1.FormattedValue")]
        public string ScheduledEnd { get; set; }
        public string Description { get; set; }
        [JsonProperty("new_meetingmemo")]
        public string Memo { get; set; }
        public string Location { get; set; }
        public string ActivityId { get; set; }
        [JsonProperty("_regardingobjectid_value")]
        public string RegardingObjectId { get; set; }
        [JsonProperty("_regardingobjectid_value@OData.Community.Display.V1.FormattedValue")]
        public string RegardingObjectName { get; set; }
        public List<Contact> Attendees { get; set; }
        public List<File> Files { get; set; }
        public Account Account { get; set; }
    }
}
