namespace Us.Ochlocracy.Model.Api.Requests.Bills;

public class UpdateBillOpinionRequest
{
    public int BillOpinionId { get; set; }
    public string Opinion { get; set; } = string.Empty;
}