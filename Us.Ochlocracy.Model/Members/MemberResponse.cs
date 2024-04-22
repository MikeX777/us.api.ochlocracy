using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Members
{
    public class MemberResponse
    {
        [JsonPropertyName("member")]
        public Member Member { get; set; } = new();
    }
}
