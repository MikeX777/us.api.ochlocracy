namespace Us.Ochlocracy.Model.Api.Requests;

public class CreateBillReactionRequest
{
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public string Opinion { get; set; } = string.Empty;
}