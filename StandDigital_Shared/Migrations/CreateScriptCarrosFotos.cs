using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandDigital_Shared.Migrations;

public class CreateScriptCarrosFotos : IScript
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
