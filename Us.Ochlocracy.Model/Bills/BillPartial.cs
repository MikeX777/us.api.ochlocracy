using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Bills
{
    public class BillPartial
    {
        [JsonPropertyName("congress")]
        public int Congress { get; set; }
        [JsonPropertyName("latestAction")]
        public Action LatestAction { get; set; } = new Action();
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
        [JsonPropertyName("originChamber")]
        public Chamber OriginChamber { get; set; }
        [JsonPropertyName("originChamberCode")]
        public ChamberCode OriginChamberCode { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public BillType Type { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonPropertyName("updateDateIncludingText")]
        public DateTime UpdateDateIncludingText { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }


    public class Action
    {
        [JsonPropertyName("actionDate")]
        public DateTime ActionDate { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
