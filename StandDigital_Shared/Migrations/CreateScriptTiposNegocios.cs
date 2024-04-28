namespace StandDigital_Shared.Migrations;

internal class CreateScriptTiposNegocios : IScript
{
    public string  Script { get; init; } =  """
        CREATE TABLE IF NOT EXISTS "TipoNegocios" (
        	"TipoNegocioId"	INTEGER NOT NULL UNIQUE,
        	"Name"	TEXT NOT NULL,
        	PRIMARY KEY("TipoNegocioId" AUTOINCREMENT)
        )
        """;
}
