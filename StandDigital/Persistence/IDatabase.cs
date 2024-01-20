using Dapper;

namespace StandDigital.Persistence;

public interface IDatabase : IDisposable
{
    void OpenTransaction();
    void RollbackTransaction();
    void CommitTransaction();
    Task<bool> InsertAsync<T>(string query, T entity) where T : class;
    Task<IEnumerable<T>> GetAsync<T>(string query, Dictionary<string, object>? dynamicParameters = null) where T : class;
    Task<TOut> InsertAsyncAndGetIdAsync<TIn, TOut>(string query, TIn newObject, CancellationToken cancellationToken);
}
