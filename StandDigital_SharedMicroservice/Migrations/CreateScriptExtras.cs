using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptExtras : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "Extras" (
        "ExtraId"	INTEGER NOT NULL UNIQUE,
        "Nome"	TEXT NOT NULL,
        PRIMARY KEY("ExtraId" AUTOINCREMENT)
        )
        """;
}
