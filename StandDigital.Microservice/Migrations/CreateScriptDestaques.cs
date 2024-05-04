using StandDigital_Shared.Migrations;

namespace StandDigital.Microservice.Migrations;

internal class CreateScriptDestaques : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "Destaques" (
            "DestaquesId" INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL UNIQUE,
        	"DataFimDestaque"	TEXT,
            PRIMARY KEY("DestaquesId" AUTOINCREMENT)
            FOREIGN KEY("CarroId") REFERENCES "Carros"("CarroId")
            )
        """;
}
