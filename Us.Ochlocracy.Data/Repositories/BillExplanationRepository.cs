using System.Data;
using System.Net;
using Dapper;
using LanguageExt;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Data.Repositories;

public class BillExplanationRepository(IDbConnection connection) : IBillExplanationRepository
{
    public async Task<Either<Error, IEnumerable<BillExplanation>>> GetBillExplanations(string billNumber, int? highestScore = null) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () =>
            await connection.QueryAsync<BillExplanation>(
                """
                SELECT br.bill_explanation_id, br.bill_number, br.user_id, br.explanation, br.score
                    FROM bill_explanations br
                    WHERE br.bill_number = @billNumber AND (@highestScore IS NULL OR br.score < @highestScore)
                    ORDER BY br.score DESC
                    LIMIT 10;
                """, new
            {
                billNumber = billNumber,
                highestScore = highestScore,
            }),
            mapError: (ex) => Error.Create(ErrorSource.BillExplanationRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> CreateBillExplanation(string billNumber, int userId, string explanation) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    INSERT INTO bill_explanations (bill_number, user_id, explanation, score)
                    VALUES (@billNumber, @userId, @explanation, 1);
                    """, new
                    {
                        billNumber = billNumber,
                        userId = userId,
                        explanation = explanation,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillExplanationRepository, HttpStatusCode.InternalServerError, ex.Message));

    public async Task<Either<Error, Unit>> UpdateBillExplanation(long billExplanationId, string explanation) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_explanations
                    SET explanation = @explanation
                    WHERE bill_explanation_id = @billExplanationId;
                    """, new
                    {
                        billExplanationId = billExplanationId,
                        explanation = explanation,
                    });
                return Unit.Default;
            }, 
            mapError: (ex) => Error.Create(ErrorSource.BillExplanationRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> UpdateBillExplanationScore(long billExplanationId, int score) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_explanations br
                    SET br.score = @score
                    WHERE br.bill_explanation_id = @billExplanationId;
                    """, new
                    {
                        billExplanationId = billExplanationId,
                        score = score,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillExplanationRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> DeleteBillExplanation(long billExplanationId) =>
    await BaseRepository.TryFuncCatchExceptionAsync(
        async () =>
        {
            await connection.ExecuteAsync(
                """
                DELETE FROM bill_explanations
                WHERE bill_explanation_id = @billExplanationId;
                """, new
                {
                    billExplanationId = billExplanationId
                });
            return Unit.Default;
        },
        mapError: (ex) => Error.Create(ErrorSource.BillExplanationRepository, HttpStatusCode.InternalServerError, ex.Message));
}