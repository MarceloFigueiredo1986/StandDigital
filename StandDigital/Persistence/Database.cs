using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace StandDigital.Persistence;

public sealed class Database(IConfiguration configuration) : IDatabase
{
    private readonly IDbConnection _dbConnection = new SqliteConnection(configuration.GetValue<string>("DatabaseName"));
    private IDbTransaction? transaction;

    public void OpenTransaction() => transaction = _dbConnection.BeginTransaction();
    public void RollbackTransaction() => transaction?.Rollback();
    public void CommitTransaction() => transaction?.Commit();

    public async Task<IEnumerable<T>> GetAsync<T>(string query, Dictionary<string, object>? dynamicParameters = null) where T : class
         => await _dbConnection.QueryAsync<T>(
             query,
             dynamicParameters,
             transaction);

    public async Task<TOut> InsertAsyncAndGetIdAsync<TIn, TOut>(string query, TIn newObject) =>
        await _dbConnection.QuerySingleAsync<TOut>($"{query};SELECT CAST(SCOPE_IDENTITY() AS INT);", newObject, transaction);

    public async Task<bool> InsertAsync<T>(string query,T entity) where T : class
    {
        var changedValues = await _dbConnection.QuerySingleAsync<int>(query,
                 param: entity,
                 transaction: transaction
             );

        return changedValues == 1;
    }

    public void Dispose()
    {
        _dbConnection.Dispose();
        transaction?.Dispose();
    }
}