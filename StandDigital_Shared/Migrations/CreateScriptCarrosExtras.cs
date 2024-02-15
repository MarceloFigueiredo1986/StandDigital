namespace StandDigital_Shared.Migrations;

internal class CreateScriptCarrosExtras : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "CarrosExtras" (
        	"CarroExtraId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"Extra"	TEXT NOT NULL,
        	PRIMARY KEY("CarroExtraId"),
        	FOREIGN KEY("CarroId") REFERENCES "Carros"("CarroId")
        )
        """;
}
