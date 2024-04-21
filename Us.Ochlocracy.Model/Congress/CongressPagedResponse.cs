using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Congress
{
    public class CongressPagedResponse
    {
        [JsonPropertyName("congresses")]
        public IEnumerable<Congress> Congresses { get; set; } = [];
    }
}
