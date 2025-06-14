namespace Us.Ochlocracy.Model.Api.Requests.Bills;

public class CreateBillExplanationRequest
{
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Explanation { get; set; } = string.Empty;
}