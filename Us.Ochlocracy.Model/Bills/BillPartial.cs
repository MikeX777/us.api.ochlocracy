namespace Us.Ochlocracy.Model.Bills
{
    public class BillPartial
    {
        public int Congress { get; set; }
        public Action LatestAction { get; set; } = new Action();
        public string Number { get; set; } = string.Empty;
        public Chamber OriginChamber { get; set; }
        public ChamberCode OriginChamberCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public BillType Type { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime UpdateDateIncludingText { get; set; }
        public string Url { get; set; } = string.Empty;
    }


    public class Action
    {
        public DateTime ActionDate { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
