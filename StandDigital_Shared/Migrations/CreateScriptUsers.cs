using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandDigital_Shared.Migrations;

internal class CreateScriptUsers : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "Users" (
        	"UserId"	INTEGER NOT NULL UNIQUE,
        	"Name"	TEXT NOT NULL,
        	"StandName"	TEXT,
        	"Address"	TEXT,
        	"NIF"	TEXT UNIQUE,
        	"NIFValidated"	INTEGER NOT NULL,
        	"Email"	TEXT NOT NULL UNIQUE,
        	"Mobile"	TEXT NOT NULL,
        	"EmailValidated"	INTEGER NOT NULL,
        	"Password"	TEXT NOT NULL,
        	"Ativo"	INTEGER NOT NULL,
        	PRIMARY KEY("UserId" AUTOINCREMENT)
        )
        """;
}
