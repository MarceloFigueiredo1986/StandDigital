using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using StandDigital_Shared.Persistence;

namespace StandDigital_Shared.Migrations;

public class MigrationsService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public async Task RunMigrations()
    {
        using var dbConn = new SqliteConnection(configuration.GetValue<string>("DatabaseName"));

        await dbConn.ExecuteAsync(sql: new CreateScriptCarros().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptCarrosExtras().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptCarrosFotos().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptCarrosMetadata().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptHistoricoNegocios().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptTiposNegocios().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptUsers().Script);
    }
}
