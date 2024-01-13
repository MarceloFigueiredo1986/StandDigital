using StandDigital.Models;
using StandDigital.Persistence;
using StandDigital.Entities;

namespace StandDigital.Repositories;

public class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel)
         => await _dbcontext.GetAsync<CarCardModel>("""
            Select * from Carros where Combustivel = @combustivel
            """,
            new Dictionary<string, object>() { { "combustivel", fuel } });

    public async Task<IEnumerable<CarEntity>> GetAllCardCarsAsync()
     => await _dbcontext.GetAsync<CarEntity>("""
            Select * from Carros
            """);

    public async Task InsertCarAsync(CarEntity carEntity, CarMetadataEntity carMetadata)
    {
        _dbcontext.OpenTransaction();
        try
        {
            var createdCarId = await _dbcontext.InsertAsyncAndGetIdAsync<CarEntity,int>("""
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
            """, carEntity);

            if (createdCarId == 0)
                throw new Exception("Car not created");

            var createdCarMetadataId = await _dbcontext.InsertAsyncAndGetIdAsync<CarMetadataEntity, int>(
                """
            INSERT INTO 
            CarrosMetadata
            VALUES
            (
            @CarroId,
            @DataMatricula,
            @DataVenda,
            @Ativo,
            @CliquesAnuncio
            )
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """, carMetadata);

            if (createdCarMetadataId == 0)
                throw new Exception("CarMetadata not created");

            _dbcontext.CommitTransaction();
        }
        catch (Exception)
        {
            _dbcontext.RollbackTransaction();
            throw;
        }
    }
}
