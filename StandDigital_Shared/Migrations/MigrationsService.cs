using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StandDigital_Shared.Migrations;

public class MigrationsService(IConfiguration configuration, ILogger<MigrationsService> logger)
{
    public async Task RunMigrations()
    {
        using var dbConn = new SqliteConnection(configuration.GetValue<string>("DatabaseName")??throw new SqliteException("Empty database connection string",404));

        try
        {
            await dbConn.ExecuteAsync(sql: new CreateScriptCarros().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptCarrosExtras().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptCarrosFotos().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptCarrosMetadata().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptHistoricoNegocios().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptTiposNegocios().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptUsers().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptPayments().Script);
            await dbConn.ExecuteAsync(sql: new CreateScriptDestaques().Script);
            await dbConn.ExecuteAsync(sql: new FillScriptTiposNegocios().Script);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(MigrationsService));
            throw;
        }
    }
}
