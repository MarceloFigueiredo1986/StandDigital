using ErrorOr;
using Microsoft.Data.Sqlite;
using StandDigital.Core.Encryption;
using StandDigital.Core.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Core.Database.Repositories.User;

internal sealed class UserRepository(IDatabase database) : IUserRepository
{
    private readonly IDatabase _dbcontext = database;

    public async Task<ErrorOr<int>> CreateAsync(UserEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var de = new DataEncryption();

            value.Address = DataEncryption.Encrypt(value.Address);
            value.NIF = DataEncryption.Encrypt(value.NIF);
            value.Email = DataEncryption.Encrypt(value.Email);
            value.Mobile = DataEncryption.Encrypt(value.Mobile);
            value.Password = de.Hash(value.Password);
            value.Username = de.Hash(value.Username);

            var createdBrandId = await _dbcontext.InsertAsyncAndGetIdAsync<UserEntity, int>(
                     """
            INSERT INTO 
            users 
            (Name,Address,NIF,Email,Mobile,EmailValidated,Username,Password,Ativo) 
            VALUES (@Name,@Address,@NIF,@Email,@Mobile,0,@Username,@Password,@Ativo)
            """, value, cancellationToken);

            if (createdBrandId == 0)
                throw new Exception($"{nameof(UserEntity)} not created");

            return createdBrandId;
        }
        catch(SqliteException sqlex) when(sqlex.Message.Contains("UNIQUE constraint failed"))
        {
            return Error.Validation(description: $"Já existe um registo com este {sqlex.Message.Substring(sqlex.Message.LastIndexOf(':') + 1)}");
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex.Message}");
        }
    }


    public Task<IEnumerable<UserEntity>> GetAllAsync() => throw new NotImplementedException();

    public async Task<UserEntity?> ValidateLogin(string user, string password)
    {
        var hashService = new DataEncryption();
        var hashedUser = hashService.Hash(user);
        var hashedPassword = hashService.Hash(password);

        var result = await _dbcontext.GetAsync<UserEntity>("""
            Select * FROM Users WHERE Username = @username and Password = @password
            """,
            new Dictionary<string, object>()
            {
                { "username", hashedUser },
                { "password", hashedPassword }
            });

        if (!result.Any()) return default;

        var userDetails = result.First();

        userDetails.Address = DataEncryption.Decrypt(result.First().Address);
        userDetails.NIF = DataEncryption.Decrypt(result.First().NIF);
        userDetails.Email = DataEncryption.Decrypt(result.First().Email);
        userDetails.Mobile = DataEncryption.Decrypt(result.First().Mobile);
        return userDetails;
    }
}
