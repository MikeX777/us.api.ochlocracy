using System.ComponentModel.DataAnnotations.Schema;

namespace Us.Ochlocracy.Data.Entities;

public class BillReactionEntity
{
    public long BillReactionId { get; init; }
    public string BillNumber { get; init; } = string.Empty;
    public long UserId { get; init; }
    public string Explanation { get; init; } = string.Empty;
    public string Opinion { get; init; } = string.Empty;
    public int Score { get; init; }
}