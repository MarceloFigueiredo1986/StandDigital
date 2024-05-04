
using StandDigital_Shared.Migrations;

namespace StandDigital.Microservice.Migrations;

internal class CreateScriptCarrosFotos : IScript
{
    public string Script { get; init; } = """
        CREATE TABLE IF NOT EXISTS "CarrosFotos" (
        	"FotoId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"Name"	TEXT NOT NULL,
        	"Data"	BLOB NOT NULL,
        	FOREIGN KEY("CarroId") REFERENCES "CarrosFotos"("CarroId"),
        	PRIMARY KEY("FotoId" AUTOINCREMENT)
        )
        """;
}
