using StandDigital.Models;
using ErrorOr;
using StandDigital_Shared.Persistence;
using StandDigital.Entities;
using StandDigital.Enums;

namespace StandDigital.Services.Repositories.Car;

public class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(Combustivel fuel)
         => await _dbcontext.GetAsync<CarCardModel>("""
            Select * from Carros c inner join CarrosMetadata cm on c.CarroId = cm.CarroId where Combustivel = @combustivel
            """,
            new Dictionary<string, object>() { { "combustivel", fuel.GetDescription() } });

    public async Task<IEnumerable<CarEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarEntity>("""
            Select * from Carros
            """);

    public async Task<ErrorOr<int>> CreateAsync(CarEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarId = await _dbcontext.InsertAsyncAndGetIdAsync<CarEntity, int>("""
            INSERT INTO Carros
            VALUES 
            (
            @Marca,
            @Modelo,
            @Combustivel,
            @ImageMin,
            @Kms,
            @Cvs,
            @Bateria,
            @TipoCarregamento,
            @Cilindrada,
            @Portas,
            @Cor,
            @Preco,
            @MesRegisto,
            @Lotacao,
            @Caixa,
            @Condicao)
            """,
            value, cancellationToken);

            if (createdCarId == 0)
                return Error.Failure(description: "Car not created");

            return createdCarId;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex}");
        }
    }

}
