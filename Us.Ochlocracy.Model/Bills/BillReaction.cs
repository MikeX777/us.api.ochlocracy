namespace Us.Ochlocracy.Model.Bills;

public class BillReaction
{
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public string Opinion { get; set; } = string.Empty;
}