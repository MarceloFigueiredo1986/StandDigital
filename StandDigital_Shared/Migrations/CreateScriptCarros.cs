namespace StandDigital_Shared.Migrations;

internal class CreateScriptCarros : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "Carros" (
        	"CarroId"	INTEGER NOT NULL UNIQUE,
        	"Marca"	TEXT NOT NULL,
        	"Modelo"	TEXT NOT NULL,
        	"Combustivel"	NUMERIC NOT NULL,
        	"FotoId"	INTEGER,
        	"Kms"	INTEGER NOT NULL,
        	"Cv"	TEXT NOT NULL,
        	"Bateria"	INTEGER,
        	"TipoCarregamento"	TEXT,
        	"Cilindrada"	INTEGER,
        	"Portas"	TEXT NOT NULL,
        	"Cor"	INTEGER,
        	"Preco"	NUMERIC NOT NULL,
        	"MesRegisto"	TEXT NOT NULL,
        	"Lotacao"	TEXT NOT NULL,
        	"Caixa"	TEXT,
        	"Ano"	INTEGER NOT NULL DEFAULT 0,
        	"Matricula"	TEXT NOT NULL,
        	"Comentarios"	TEXT,
        	"PrecisaPneus"	INTEGER NOT NULL DEFAULT 0,
        	"PrimeiroDono"	INTEGER NOT NULL DEFAULT 1,
        	"Danos"	BLOB,
        	"ProblemasMotor"	TEXT,
        	"Nacional"	INTEGER NOT NULL DEFAULT 1,
        	"Ativo"	INTEGER NOT NULL DEFAULT 1,
            PRIMARY KEY("CarroId" AUTOINCREMENT)
        )
        """;
}
