using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptModelos : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "Modelos" (
        "Nome"	TEXT NOT NULL UNIQUE,
        "MarcaId"	INTEGER NOT NULL,
        FOREIGN KEY("MarcaId") REFERENCES "Marcas"("MarcaId")
        )
        """;
}
