using Newtonsoft.Json;

namespace Us.Ochlocracy.Model.Bills
{
    public class Bill
    {
        [JsonProperty("actions")]
        public ActionLink Action { get; set; } = new();
        [JsonProperty("amendments")]
        public AmendmentLink Amendments { get; set; } = new();
        [JsonProperty("cboCostEstimates")]
        public IEnumerable<CboCostEstimate> CboCostEstimates { get; set; } = [];
        [JsonProperty("committeeReports")]
        public IEnumerable<CommitteeReport> CommitteeReports { get; set; } = [];
        [JsonProperty("committees")]
        public Committee Committees { get; set; } = new();
        [JsonProperty("congress")]
        public int Congress { get; set; }
        [JsonProperty("constitutionalAuthorityStatementText")]
        public string ConstitutionalAuthorityStatementText { get; set; } = string.Empty;
        [JsonProperty("cosponsors")]
        public Cosponsor Cosponsors { get; set; } = new();
        [JsonProperty("introducedDate")]
        public DateTime IntroducedDate { get; set; }
        [JsonProperty("latestAction")]
        public Action LastAction { get; set; } = new();
        [JsonProperty("laws")]
        public IEnumerable<Law> Laws { get; set; } = [];
        [JsonProperty("number")]
        public string Number { get; set; } = string.Empty;
        [JsonProperty("originChamber")]
        public Chamber OriginChamber { get; set; }
        [JsonProperty("policyArea")]
        public PolicyArea PolicyArea { get; set; } = new();
        [JsonProperty("relatedBills")]
        public RelatedBill RelatedBills { get; set; } = new();
        [JsonProperty("sponsors")]
        public IEnumerable<Sponsor> Sponsors { get; set; } = [];
        [JsonProperty("subjects")]
        public Subject Subjects { get; set; } = new();
        [JsonProperty("summaries")]
        public Summary Summaries { get; set; } = new();
        [JsonProperty("textVersions")]
        public TextVersion TextVersions { get; set; } = new();
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("titles")]
        public Title Titles { get; set; } = new();
        [JsonProperty("type")]
        public BillType Type { get; set; }
        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonProperty("updateDateIncludingText")]
        public DateTime UpdateDateIncludingText { get; set; }

    }

    public class CboCostEstimate
    {
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty("pubDate")]
        public DateTime PubDate { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class CommitteeReport
    {
        [JsonProperty("citation")]
        public string Citation { get; set; } = string.Empty;
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }


    public class Law
    {
        [JsonProperty("number")]
        public string Number { get; set; } = string.Empty;
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class PolicyArea
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Sponsor
    {
        [JsonProperty("bioguideId")]
        public string BioguideId { get; set; } = string.Empty;
        [JsonProperty("district")]
        public int District { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonProperty("fullName")]
        public string FullName { get; set; } = string.Empty;
        [JsonProperty("isByRequest")]
        public string IsByRequest { get; set; } = string.Empty; // TODO: Possibly make enum? "N"
        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;
        [JsonProperty("middleName")]
        public string MiddleName { get; set; } = string.Empty;
        [JsonProperty("party")]
        public string Party { get; set; } = string.Empty; // TODO: possibly make enum? "D"
        [JsonProperty("state")]
        public string State { get; set; } = string.Empty; // TODO: possibly make enum? "NY"
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class CountLink
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }
    public class ActionLink : CountLink { }
    public class AmendmentLink : CountLink { }
    public class Committee : CountLink { }
    public class Cosponsor : CountLink
    {
        [JsonProperty("countIncludingWithdrawnCosponsors")]
        public int CountIncludingWithdrawnCosponsors { get; set; }
    }
    public class RelatedBill : CountLink { }
    public class Subject : CountLink { }
    public class Summary : CountLink { }
    public class TextVersion : CountLink { }
    public class Title : CountLink { }

}
