using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Bills
{
    public class BillResponse
    {
        [JsonPropertyName("bills")]
        public IEnumerable<Bill> Bills { get; set; } = [];
    }
}
