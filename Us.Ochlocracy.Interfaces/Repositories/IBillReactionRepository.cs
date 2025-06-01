using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Interfaces.Repositories;

public interface IBillReactionRepository
{
    Task<Either<Error, IEnumerable<BillReaction>>> GetBillReactions(string billNumber, int? highestScore = null);
    Task<Either<Error, Unit>> CreateBillReaction(string billNumber, int userId, string explanation, string opinion);
    Task<Either<Error, Unit>> UpdateBillReaction(BillReaction billReaction);
    Task<Either<Error, Unit>> UpdateBillReactionScore(long billReactionId, int score);
    Task<Either<Error, Unit>> DeleteBillReaction(long billReactionId);
}