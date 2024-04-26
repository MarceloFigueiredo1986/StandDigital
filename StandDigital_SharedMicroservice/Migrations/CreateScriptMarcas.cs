using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptMarcas : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "Marcas" (
        	"MarcaId"	INTEGER NOT NULL UNIQUE,
        	"Nome"	TEXT NOT NULL,
        	"Image"	TEXT,
        	PRIMARY KEY("MarcaId")
        )
        """;
}
