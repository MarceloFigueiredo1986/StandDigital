using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptModelos : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "Modelos" (
        "ModeloId"	INTEGER NOT NULL UNIQUE,
        "Nome"	TEXT NOT NULL,
        "Cilindrada"	TEXT,
        "Cavalagem"	INTEGER NOT NULL,
        "MarcaId"	INTEGER NOT NULL,
        "Combustivel"	TEXT NOT NULL DEFAULT 'Diesel',
        "Segmento"	TEXT,
        PRIMARY KEY("ModeloId" AUTOINCREMENT)
        )
        """;
}
