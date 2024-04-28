using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace StandDigital_Shared.Migrations;

public class MigrationsService(IConfiguration configuration)
{
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
        await dbConn.ExecuteAsync(sql: new CreateScriptPayments().Script);
        await dbConn.ExecuteAsync(sql: new CreateScriptDestaques().Script);
        await dbConn.ExecuteAsync(sql: new FillScriptTiposNegocios().Script);
    }
}
