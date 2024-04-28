
namespace StandDigital_Shared.Migrations;

internal class CreateScriptUsers : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "Users" (
        	"UserId"	INTEGER NOT NULL UNIQUE,
        	"Name"	TEXT NOT NULL,
        	"Address"	TEXT,
        	"NIF"	TEXT UNIQUE,
        	"Email"	TEXT NOT NULL UNIQUE,
        	"Mobile"	TEXT NOT NULL,
        	"EmailValidated"	INTEGER NOT NULL,
        	"Username"	TEXT NOT NULL,
        	"Password"	TEXT NOT NULL,
        	"Ativo"	INTEGER NOT NULL,
        	PRIMARY KEY("UserId" AUTOINCREMENT)
        )
        """;
}
