using StandDigital.Models;
using ErrorOr;
using StandDigital_Shared.Persistence;
using StandDigital.Entities;

namespace StandDigital.Repositories.Car;

public class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel)
         => await _dbcontext.GetAsync<CarCardModel>("""
            Select * from Carros where Combustivel = @combustivel
            """,
            new Dictionary<string, object>() { { "combustivel", fuel } });

    public async Task<IEnumerable<CarEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarEntity>("""
            Select * from Carros
            """);

    public async Task<ErrorOr<int>> CreateAsync(CarEntity value,CancellationToken cancellationToken)
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
