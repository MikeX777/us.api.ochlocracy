using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Interfaces.Repositories;

public interface IBillExplanationRepository
{
    Task<Either<Error, IEnumerable<BillExplanation>>> GetBillExplanations(string billNumber, int? highestScore = null);
    Task<Either<Error, Unit>> CreateBillExplanation(string billNumber, int userId, string explanation);
    Task<Either<Error, Unit>> UpdateBillExplanation(long billExplanationId, string explanation);
    Task<Either<Error, Unit>> UpdateBillExplanationScore(long billExplanationId, int score);
    Task<Either<Error, Unit>> DeleteBillExplanation(long billExplanationId);
}