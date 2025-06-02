namespace Us.Ochlocracy.Model.Bills;

public class BillExplanation
{
    public long BillExplanationId { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public int Score { get; set; }
}