namespace Us.Ochlocracy.Model.Bills
{
    public class Bill
    {
        public Action Action { get; set; } = new();
        public AmendmentLink Amendments { get; set; } = new();
        public IEnumerable<CboCostEstimate> CboCostEstimates { get; set; } = [];
        public IEnumerable<CommitteeReport> CommitteeReports { get; set; } = [];
        public Committee Committees { get; set; } = new();
        public int Congress { get; set; }
        public string ConstitutionalAuthorityStatementText { get; set; } = string.Empty;
        public Cosponsor Cosponsors { get; set; } = new();
        public DateTime IntroducedDate { get; set; }
        public Action LastAction { get; set; } = new();
        public IEnumerable<Law> Laws { get; set; } = [];
        public string Number { get; set; } = string.Empty;
        public Chamber OriginChamber { get; set; }
        public PolicyArea PolicyArea { get; set; } = new();
        public RelatedBill RelatedBills { get; set; } = new();
        public IEnumerable<Sponsor> Sponsors { get; set; } = [];
        public Subject Subjects { get; set; } = new();
        public Summary Summaries { get; set; } = new();
        public TextVersion TextVersions { get; set; } = new();
        public string Title { get; set; } = string.Empty;
        public Title Titles { get; set; } = new();
        public BillType Type { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime UpdateDateIncludingText { get; set; }

    }


    public class CboCostEstimate
    {
        public string Description { get; set; } = string.Empty;
        public DateTime PubDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class CommitteeReport
    {
        public string Citation { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }


    public class Law
    {
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class PolicyArea
    {
        public string Name { get; set; } = string.Empty;
    }

    public class Sponsor
    {
        public string BioguideId { get; set; } = string.Empty;
        public int District { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string IsByRequest { get; set; } = string.Empty; // TODO: Possibly make enum? "N"
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty; // TODO: possibly make enum? "D"
        public string State { get; set; } = string.Empty; // TODO: possibly make enum? "NY"
        public string Url { get; set; } = string.Empty;
    }

    public class CountLink
    {
        public int Count { get; set; }
        public string Url { get; set; } = string.Empty;
    }
    public class ActionLink : CountLink { }
    public class AmendmentLink : CountLink { }
    public class Committee : CountLink { }
    public class Cosponsor : CountLink
    {
        public int CountIncludingWithdrawnCosponsors { get; set; }
    }
    public class RelatedBill : CountLink { }
    public class Subject : CountLink { }
    public class Summary : CountLink { }
    public class TextVersion : CountLink { }
    public class Title : CountLink { }

}
