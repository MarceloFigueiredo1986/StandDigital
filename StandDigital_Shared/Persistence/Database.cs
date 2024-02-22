using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace StandDigital_Shared.Persistence;

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

    public async Task<IEnumerable<T>> GetAsync<T,T1>(string query,string splitOn, Func<T,T1,T> mapFunction, 
        Dictionary<string, object>? dynamicParameters = null) 
        where T : class
        where T1 : class
     => await _dbConnection.QueryAsync(
         sql: query,
         map: mapFunction,
         param: dynamicParameters,
         splitOn: splitOn);

    public async Task<TOut> InsertAsyncAndGetIdAsync<TIn, TOut>(string query, TIn newObject,CancellationToken cancellationToken) =>
        await _dbConnection.QuerySingleAsync<TOut>(new CommandDefinition(
            commandText : $"{query};SELECT CAST(SCOPE_IDENTITY() AS INT);",
            parameters: newObject,
            transaction: transaction,
            cancellationToken: cancellationToken));

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

    public async Task<SqlMapper.GridReader> GetAsync(
        string[] queries, 
        Dictionary<string, object>? dynamicParameters = null)
          => await _dbConnection.QueryMultipleAsync(
             $"{string.Join(' ',queries)}",
             dynamicParameters,
             transaction);

    public Task<int> UpdateAsync<TIn>(string query, TIn obj, CancellationToken cancellationToken)
        => _dbConnection.ExecuteAsync(new CommandDefinition(
            commandText: query,
            parameters: obj,
            cancellationToken: cancellationToken));
}