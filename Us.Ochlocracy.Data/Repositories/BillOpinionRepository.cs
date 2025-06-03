using System.Data;
using System.Net;
using Dapper;
using LanguageExt;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Data.Repositories;

public class BillOpinionRepository(IDbConnection connection) : IBillOpinionRepository
{
    public async Task<Either<Error, IEnumerable<BillOpinion>>> GetBillOpinions(string billNumber, int? highestScore = null) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () =>
            await connection.QueryAsync<BillOpinion>(
                """
                SELECT bo.bill_opinion_id, bo.bill_number, bo.user_id, bo.opinion, bo.score
                    FROM bill_opinions bo
                    WHERE bo.bill_number = @billNumber AND (@highestScore IS NULL OR bo.score < @highestScore)
                    ORDER BY bo.score DESC
                    LIMIT 10;
                """, new
            {
                billNumber = billNumber,
                highestScore = highestScore,
            }),
            mapError: (ex) => Error.Create(ErrorSource.BillOpinionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> CreateBillOpinion(string billNumber, int userId, string opinion) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    INSERT INTO bill_opinions (bill_number, user_id, opinion, score)
                    VALUES (@billNumber, @userId, @opinion, 1);
                    """, new
                    {
                        billNumber = billNumber,
                        userId = userId,
                        opinion = opinion,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillOpinionRepository, HttpStatusCode.InternalServerError, ex.Message));

    public async Task<Either<Error, Unit>> UpdateBillOpinion(long billOpinionId, string opinion) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_opinions
                    SET opinion = @opinion
                    WHERE bill_opinion_id = @billOpinionId;
                    """, new
                    {
                        billOpinionId = billOpinionId,
                        opinion = opinion,
                    });
                return Unit.Default;
            }, 
            mapError: (ex) => Error.Create(ErrorSource.BillOpinionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> UpdateBillOpinionScore(long billOpinionId, int score) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_opinions br
                    SET br.score = @score
                    WHERE br.bill_opinion_id = @billOpinionId;
                    """, new
                    {
                        billOpinionId = billOpinionId,
                        score = score,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillOpinionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> DeleteBillOpinion(long billOpinionId) =>
    await BaseRepository.TryFuncCatchExceptionAsync(
        async () =>
        {
            await connection.ExecuteAsync(
                """
                DELETE FROM bill_opinions
                WHERE bill_opinion_id = @billOpinionId;
                """, new
                {
                    billOpinionId = billOpinionId
                });
            return Unit.Default;
        },
        mapError: (ex) => Error.Create(ErrorSource.BillOpinionRepository, HttpStatusCode.InternalServerError, ex.Message));
}