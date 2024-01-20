using ErrorOr;

namespace StandDigital.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<ErrorOr<int>> CreateAsync(T value, CancellationToken cancellationToken);
}
