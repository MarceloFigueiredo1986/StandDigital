
using StandDigital_Shared.Migrations;

namespace StandDigital.Microservice.Migrations;

internal class CreateScriptCarrosMetadata : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "CarrosMetadata" (
        	"CarroMetadataId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"DataInicioAnuncio"	TEXT,
        	"DataFimAnuncio"	TEXT,
        	"VendedorId"	INTEGER,
        	"CompradorId"	INTEGER,
        	"TipoNegocioId"	INTEGER,
        	"Paid"	INTEGER,
        	FOREIGN KEY("CarroId") REFERENCES "Carros"("CarroId"),
        	PRIMARY KEY("CarroMetadataId" AUTOINCREMENT)
        )
        """;
}
