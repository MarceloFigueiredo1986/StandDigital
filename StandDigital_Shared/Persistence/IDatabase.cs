using Dapper;

namespace StandDigital_Shared.Persistence;

public interface IDatabase : IDisposable
{
    Task OpenTransactionAsync();
    void RollbackTransaction();
    void CommitTransaction();
    Task<bool> InsertAsync<T>(string query, T entity) where T : class;
    Task<IEnumerable<T>> GetAsync<T>(string query, Dictionary<string, object>? dynamicParameters = null) where T : class;

    /// <summary>
    /// Use when want to run multiple queries with same connection
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="dynamicParameters"></param>
    /// <returns></returns>
    Task<SqlMapper.GridReader> GetAsync(
        string[] queries,
        Dictionary<string, object>? dynamicParameters = null);

    /// <summary>
    /// Use when want to split the results from on property forward and map it into a new object, creating a nested list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="query"></param>
    /// <param name="splitOn"></param>
    /// <param name="mapFunction"></param>
    /// <param name="dynamicParameters"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAsync<T,T1>(string query, string splitOn, Func<T,T1,T> mapFunction,
        Dictionary<string, object>? dynamicParameters = null) where T : class where T1: class;

    Task<T?> GetSingleAsync<T>(string query, Dictionary<string, object> parameters);

    Task<TOut> InsertAsyncAndGetIdAsync<TIn,TOut>(string query,TIn value, CancellationToken cancellationToken);

    Task<int> UpdateAsync<TIn>(string query, TIn obj, CancellationToken cancellationToken);
}
