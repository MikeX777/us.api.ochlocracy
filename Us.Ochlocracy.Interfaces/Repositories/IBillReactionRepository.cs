using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Interfaces.Repositories;

public interface IBillReactionRepository
{
    Task<Either<Error, IEnumerable<BillReaction>>> GetBillReactions(string billNumber, int? highestScore = null);
}