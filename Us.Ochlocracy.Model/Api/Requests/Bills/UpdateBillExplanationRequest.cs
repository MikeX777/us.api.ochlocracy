namespace Us.Ochlocracy.Model.Api.Requests.Bills;

public class UpdateBillExplanationRequest
{
    public int BillExplanationId { get; set; }
    public string Explanation { get; set; } = string.Empty;
}