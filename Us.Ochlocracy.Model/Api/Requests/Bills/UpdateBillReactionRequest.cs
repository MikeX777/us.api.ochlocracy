namespace Us.Ochlocracy.Model.Api.Requests.Bills;

public class UpdateBillReactionRequest
{
    public int BillReactionId { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public string Opinion { get; set; } = string.Empty;
}