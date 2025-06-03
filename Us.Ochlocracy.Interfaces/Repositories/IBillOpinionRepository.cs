using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Interfaces.Repositories;

public interface IBillOpinionRepository
{
    Task<Either<Error, IEnumerable<BillOpinion>>> GetBillOpinions(string billNumber, int? highestScore = null);
    Task<Either<Error, Unit>> CreateBillOpinion(string billNumber, int userId, string opinion);
    Task<Either<Error, Unit>> UpdateBillOpinion(long billOpinionId, string opinion);
    Task<Either<Error, Unit>> UpdateBillOpinionScore(long billOpinionId, int score);
    Task<Either<Error, Unit>> DeleteBillOpinion(long billOpinionId);
}