using System.Text.Json.Serialization;

namespace Us.Ochlocracy.Model.Bills
{
    public class Bill
    {
        [JsonPropertyName("action")]
        public Action Action { get; set; } = new();
        [JsonPropertyName("amendments")]
        public AmendmentLink Amendments { get; set; } = new();
        [JsonPropertyName("cboCostEstimates")]
        public IEnumerable<CboCostEstimate> CboCostEstimates { get; set; } = [];
        [JsonPropertyName("committeeReports")]
        public IEnumerable<CommitteeReport> CommitteeReports { get; set; } = [];
        [JsonPropertyName("committees")]
        public Committee Committees { get; set; } = new();
        [JsonPropertyName("congress")]
        public int Congress { get; set; }
        [JsonPropertyName("constitutionalAuthorityStatementText")]
        public string ConstitutionalAuthorityStatementText { get; set; } = string.Empty;
        [JsonPropertyName("cosponsors")]
        public Cosponsor Cosponsors { get; set; } = new();
        [JsonPropertyName("introducedDate")]
        public DateTime IntroducedDate { get; set; }
        [JsonPropertyName("latestAction")]
        public Action LastAction { get; set; } = new();
        [JsonPropertyName("laws")]
        public IEnumerable<Law> Laws { get; set; } = [];
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
        [JsonPropertyName("originChamber")]
        public Chamber OriginChamber { get; set; }
        [JsonPropertyName("policyArea")]
        public PolicyArea PolicyArea { get; set; } = new();
        [JsonPropertyName("relatedBills")]
        public RelatedBill RelatedBills { get; set; } = new();
        [JsonPropertyName("sponsors")]
        public IEnumerable<Sponsor> Sponsors { get; set; } = [];
        [JsonPropertyName("subjects")]
        public Subject Subjects { get; set; } = new();
        [JsonPropertyName("summaries")]
        public Summary Summaries { get; set; } = new();
        [JsonPropertyName("textVersions")]
        public TextVersion TextVersions { get; set; } = new();
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("titles")]
        public Title Titles { get; set; } = new();
        [JsonPropertyName("type")]
        public BillType Type { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonPropertyName("updateDateIncludingText")]
        public DateTime UpdateDateIncludingText { get; set; }

    }


    public class CboCostEstimate
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("pubDate")]
        public DateTime PubDate { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class CommitteeReport
    {
        [JsonPropertyName("citation")]
        public string Citation { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }


    public class Law
    {
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class PolicyArea
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Sponsor
    {
        [JsonPropertyName("bioguidId")]
        public string BioguideId { get; set; } = string.Empty;
        [JsonPropertyName("district")]
        public int District { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;
        [JsonPropertyName("isByRequest")]
        public string IsByRequest { get; set; } = string.Empty; // TODO: Possibly make enum? "N"
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; } = string.Empty;
        [JsonPropertyName("party")]
        public string Party { get; set; } = string.Empty; // TODO: possibly make enum? "D"
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty; // TODO: possibly make enum? "NY"
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class CountLink
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
    public class ActionLink : CountLink { }
    public class AmendmentLink : CountLink { }
    public class Committee : CountLink { }
    public class Cosponsor : CountLink
    {
        [JsonPropertyName("countIncludingWithdrawnCosponsors")]
        public int CountIncludingWithdrawnCosponsors { get; set; }
    }
    public class RelatedBill : CountLink { }
    public class Subject : CountLink { }
    public class Summary : CountLink { }
    public class TextVersion : CountLink { }
    public class Title : CountLink { }

}
