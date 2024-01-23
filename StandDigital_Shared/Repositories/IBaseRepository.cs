using ErrorOr;

namespace StandDigital_Shared.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<ErrorOr<int>> CreateAsync(T value, CancellationToken cancellationToken);
}
