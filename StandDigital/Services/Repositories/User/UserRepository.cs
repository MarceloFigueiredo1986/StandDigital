using StandDigital.Encryption;
using StandDigital.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Services.Repositories.User;

public sealed class UserRepository(IDatabase database) : IUserRepository
{
    private readonly IDatabase _dbcontext = database;

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
        userDetails.Role = DataEncryption.Decrypt(result.First().Role);
        return userDetails;
    }
}
