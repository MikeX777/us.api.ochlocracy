using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Congress
{
    public class CongressResponse
    {
        [JsonPropertyName("congress")]
        public Congress Congress { get; set; } = new();
    }
}
