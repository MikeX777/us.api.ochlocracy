using System.Data;
using System.Net;
using Dapper;
using LanguageExt;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model;

namespace Us.Ochlocracy.Data.Repositories;


public class UserRepository : IUserRepository
{
    private readonly IDbConnection connection;

    public UserRepository(IDbConnection connection) => this.connection = connection;

    public async Task<Either<Error, UserEntity>> GetUser(string username) =>
        await BaseRepository.TryFuncCatchExceptionAsync(async () => 
            (await connection.QueryAsync<UserEntity>(
                """
                SELECT u.user_id, u.username, u.given_name, u.family_name, u.created_at
                    FROM users u 
                    WHERE u.username = @username
                """, new
                {
                    username = username
                })).FirstOrDefault(),
        mapError: (ex) => Error.Create(ErrorSource.BillReactionRepository, HttpStatusCode.InternalServerError, ex.Message));
}

