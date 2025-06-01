using System.Data;
using System.Net;
using Dapper;
using LanguageExt;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Data.Repositories;

public class BillReactionRepository(IDbConnection connection) : IBillReactionRepository
{
    public async Task<Either<Error, IEnumerable<BillReaction>>> GetBillReactions(string billNumber, int? highestScore = null) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () =>
            await connection.QueryAsync<BillReaction>(
                """
                SELECT br.bill_reaction_id, br.bill_number, br.user_id, br.explanation, br.opinion, br.score
                    FROM bill_reactions br
                    WHERE br.bill_number = @billNumber AND (@highestScore IS NULL OR br.score < @highestScore)
                    ORDER BY br.score DESC
                    LIMIT 10;
                """, new
            {
                billNumber = billNumber,
                highestScore = highestScore,
            }),
            mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> CreateBillReaction(string billNumber, int userId, string explanation, string opinion) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    INSERT INTO bill_reactions (bill_number, user_id, explanation, opinion, score)
                    VALUES (@billNumber, @userId, @explanation, @opinion, 1);
                    """, new
                    {
                        billNumber = billNumber,
                        userId = userId,
                        explanation = explanation,
                        opinion = opinion,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));

    public async Task<Either<Error, Unit>> UpdateBillReaction(BillReaction billReaction) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_reactions br
                    SET br.explanation = @explanation, br.opinion = @opinion
                    WHERE br.bill_reaction_id = @billReactionId;
                    """, new
                    {
                        billReaction = billReaction.BillReactionId,
                        explanation = billReaction.Explanation,
                        opinion = billReaction.Opinion,
                    });
                return Unit.Default;
            }, 
            mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> UpdateBillReactionScore(long billReactionId, int score) =>
        await BaseRepository.TryFuncCatchExceptionAsync(
            async () =>
            {
                await connection.ExecuteAsync(
                    """
                    UPDATE bill_reactions br
                    SET br.score = @score
                    WHERE br.bill_reaction_id = @billReactionId;
                    """, new
                    {
                        billReactionId = billReactionId,
                        score = score,
                    });
                return Unit.Default;
            },
            mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));
    
    public async Task<Either<Error, Unit>> DeleteBillReaction(long billReactionId) =>
    await BaseRepository.TryFuncCatchExceptionAsync(
        async () =>
        {
            await connection.ExecuteAsync(
                """
                DELETE FROM bill_reactions
                WHERE bill_reaction_id = @billReactionId;
                """, new
                {
                    billReactionId = billReactionId
                });
            return Unit.Default;
        },
        mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));

    public async Task<Either<Error, IEnumerable<BillReactionEntity>>> GetBillReactionEntities(string billNumber,
        int? highestScore = null) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () => await connection.QueryAsync<BillReactionEntity>(
            """
            SELECT br.bill_reaction_id, br.bill_number, br.user_id, br.explanation, br.opinion, br.score
                FROM bill_reactions br
                WHERE br.bill_number = @billNumber AND (@highestScore IS NULL OR br.score < @highestScore)
                ORDER BY br.score DESC
                LIMIT 10
            """, new
            {
                billNumber = billNumber,
                highestScore = highestScore,
            }),
            mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));
}