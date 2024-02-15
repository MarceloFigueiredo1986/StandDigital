using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandDigital_Shared.Migrations;

internal class CreateScriptCarrosMetadata : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "CarrosMetadata" (
        	"CarroMetadataId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"DataVenda"	TEXT,
        	"Ativo"	INTEGER NOT NULL DEFAULT 'true',
        	"DataInicioAnuncio"	TEXT,
        	FOREIGN KEY("CarroId") REFERENCES "Carros"("CarroId"),
        	PRIMARY KEY("CarroMetadataId" AUTOINCREMENT)
        )
        """;
}
