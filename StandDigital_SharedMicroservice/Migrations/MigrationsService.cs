using Dapper;
using Microsoft.Data.Sqlite;

namespace StandDigital_SharedMicroservice.Migrations;

public class MigrationsService(IConfiguration configuration, ILogger<MigrationsService> logger)
{
    public async Task RunMigrations()
    {
        using var dbConn = new SqliteConnection(configuration.GetValue<string>("DatabaseName") ?? throw new SqliteException("Empty database connection string", 404));

        try
        {
            await dbConn.ExecuteAsync(sql: new CreateScriptExtras().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptMarcas().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptModelos().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptFillMarcas().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptFillModelos().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptFillExtras().Script);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(MigrationsService));
            throw;
        }
    }
}
