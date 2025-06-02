namespace Us.Ochlocracy.Model.Bills;

public class BillOpinion
{
    public long BillOpinionId { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Opinion { get; set; } = string.Empty;
    public int Score { get; set; }
}