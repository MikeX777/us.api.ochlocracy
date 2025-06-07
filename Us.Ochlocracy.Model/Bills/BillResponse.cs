using Newtonsoft.Json;

namespace Us.Ochlocracy.Model.Bills
{
    public class BillResponse
    {
        [JsonProperty("bill")] public Bill Bill { get; set; } = new();
    }
}
