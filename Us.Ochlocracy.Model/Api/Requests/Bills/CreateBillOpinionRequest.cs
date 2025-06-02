namespace Us.Ochlocracy.Model.Api.Requests.Bills;

public class CreateBillOpinionRequest
{
    public string BillNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Opinion { get; set; } = string.Empty;
}