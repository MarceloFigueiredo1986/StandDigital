using ErrorOr;
using StandDigital.Entities;
using StandDigital.Persistence;

namespace StandDigital.Repositories.CarMetadata;

public class CarMetadataRepository(IDatabase database) : ICarMetadataRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<ErrorOr<int>> CreateAsync(CarMetadataEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarMetadataId = await _dbcontext.InsertAsyncAndGetIdAsync<CarMetadataEntity,int>(
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
            """, value, cancellationToken);

            if (createdCarMetadataId == 0)
                throw new Exception($"{nameof(CarMetadataEntity)} not created");


            return createdCarMetadataId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<CarMetadataEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarMetadataEntity>("""
            Select * from CarrosMetadata
            """);
}
