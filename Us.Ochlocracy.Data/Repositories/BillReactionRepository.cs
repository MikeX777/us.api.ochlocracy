using System.Data;
using System.Net;
using Dapper;
using LanguageExt;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Data.Repositories;

public class BillReactionRepository : IBillReactionRepository
{
    private readonly IDbConnection connection;

    public BillReactionRepository(IDbConnection connection) => this.connection = connection;
    
    public async Task<Either<Error, IEnumerable<BillReaction>>> GetBillReactions(string billNumber, int? highestScore = null) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () =>
            await connection.QueryAsync<BillReaction>(
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