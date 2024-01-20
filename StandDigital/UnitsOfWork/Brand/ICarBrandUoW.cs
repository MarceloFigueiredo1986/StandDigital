using StandDigital.Entities;

namespace StandDigital.UnitsOfWork.Brand;

public interface ICarBrandUoW
{
    Task CheckIfExists(
        string Brand,
        string Model,
        CancellationToken cancellationToken);


}
