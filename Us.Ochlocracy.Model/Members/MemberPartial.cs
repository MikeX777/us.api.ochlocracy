using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Members
{
    public class MemberPartial
    {
        [JsonPropertyName("bioguideId")]
        public string BioguideId { get; set; } = string.Empty;
        [JsonPropertyName("depiction")]
        public Depiction Depiction { get; set; } = new();
        [JsonPropertyName("district")]
        public string? District { get; set; } // not sure what this is
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("partyName")]
        public string PartyName { get; set; } = string.Empty; // TODO: possibly make an enum "Democratic"
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty; // TODO: possibly make an enum "Vermont"
        [JsonPropertyName("terms")]
        public Term Term { get; set; } = new();
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class TermPartial
    {
        [JsonPropertyName("item")]
        public IEnumerable<TermItemPartial> Item { get; set; } = [];
    }

    public class TermItemPartial
    {
        [JsonPropertyName("chamber")]
        public Chamber Chamber { get; set; }
        [JsonPropertyName("endYear")]
        public int? EndYear { get; set; }
        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }
    }
}
