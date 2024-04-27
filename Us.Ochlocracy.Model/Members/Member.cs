using System.Text.Json.Serialization;


namespace Us.Ochlocracy.Model.Members
{
    public class Member
    {
        [JsonPropertyName("bioguideId")]
        public string BioguideId { get; set; } = string.Empty;
        [JsonPropertyName("birthYear")]
        public string BirthYear { get; set; } = string.Empty;
        [JsonPropertyName("consponsoredLegislation")]
        public CosponsoredLegislation CosponsoredLegislation { get; set; } = new();
        [JsonPropertyName("depiction")]
        public Depiction Depiction { get; set; } = new();
        [JsonPropertyName("directOrderName")]
        public string DirectOrderName { get; set; } = string.Empty;
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("honorificName")]
        public string HonorificName { get; set; } = string.Empty;
        [JsonPropertyName("invertedOrderName")]
        public string InvertedOrderName { get; set; } = string.Empty;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("leadership")]
        public IEnumerable<Leadership> Leadership { get; set; } = [];
        [JsonPropertyName("partyHistory")]
        public IEnumerable<PartyHistory> PartyHistory { get; set; } = [];
        [JsonPropertyName("sponsoredLegislation")]
        public SponsoredLegislation SponsoredLegislation { get; set; } = new();
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty; // TODO: Possibly make this an enum "Vermont"
        [JsonPropertyName("terms")]
        public IEnumerable<Term> Terms { get; set; } = [];
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; }
    }

    public class CosponsoredLegislation
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("URL")]
        public string Url { get; set; } = string.Empty;
    }

    public class Depiction
    {
        [JsonPropertyName("attribution")]
        public string Attribution { get; set; } = string.Empty;
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class Leadership
    {
        [JsonPropertyName("congress")]
        public int Congress { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class PartyHistory
    {
        [JsonPropertyName("partyAbbreviation")]
        public string PartyAbbreviation { get; set; } = string.Empty; // TODO: possibly make this an enum? "D"
        [JsonPropertyName("partyName")]
        public string PartyName { get; set; } = string.Empty; // TODO: possibly make this an enum? "Democrat"
        [JsonPropertyName("startYear")]
        public int StartYear { get; set; }
    }

    public class SponsoredLegislation
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Term
    {
        [JsonPropertyName("chamber")]
        public Chamber Chamber { get; set; }
        [JsonPropertyName("congress")]
        public int Congress { get; set; }
        [JsonPropertyName("endYear")]
        public int? EndYear { get; set; }
        [JsonPropertyName("memberType")]
        public string MemberType { get; set; } = string.Empty; // TODO: possibly make this an enum? "Senator"
        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }
        [JsonPropertyName("stateCode")]
        public string StateCode { get; set; } = string.Empty; // TODO: possibly make this an enum "VT"
        [JsonPropertyName("stateName")]
        public string StateName { get; set; } = string.Empty; // TODO: possibly make this an enum "Vermont"
    }
}

