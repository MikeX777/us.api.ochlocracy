using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Congress
{
    public class Congress
    {
        [JsonPropertyName("endYear")]
        public string EndYear { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("sessions")]
        public IEnumerable<Session> Sessions { get; set; } = [];
        [JsonPropertyName("startYear")]
        public string StartYear { get; set; } = string.Empty;
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; } // Null on paged response
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class Session
    {
        [JsonPropertyName("chamber")]
        public string Chamber { get; set; } = string.Empty; // TODO: Maybe make this an enum? "House of Representatives"
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;
        [JsonPropertyName("number")]
        public int Number { get; set; }
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty; // TODO: maybe make this an enum? "R"
    }
}
