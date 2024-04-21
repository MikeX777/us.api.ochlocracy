using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Bills
{
    public class BillPartialResponse
    {
        [JsonPropertyName("bills")]
        public IEnumerable<BillPartial> Bills { get; set; } = [];
    }
}
