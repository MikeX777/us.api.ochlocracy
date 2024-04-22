using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Members
{
    public class MemberPagedResponse
    {
        [JsonPropertyName("members")]
        public IEnumerable<MemberPartial> Members { get; set; } = [];
    }
}
