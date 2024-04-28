namespace StandDigital_Shared.Migrations;

internal class CreateScriptHistoricoNegocios : IScript
{
    public string Script { get ; init; } = """
                CREATE TABLE IF NOT EXISTS "HistoricoNegocios" (
        	"NegocioId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"Valor"	REAL NOT NULL,
        	"DataLicitacao"	TEXT NOT NULL,
        	"CompradorId"	INTEGER NOT NULL,
        	"VendedorId"	INTEGER NOT NULL,
        	"TipoNegocioId"	INTEGER NOT NULL,
        	PRIMARY KEY("NegocioId","CarroId"),
        	FOREIGN KEY("TipoNegocioId") REFERENCES "TipoNegocios"("TipoNegocioId"),
        	FOREIGN KEY("CarroId") REFERENCES "Carros"("CarroId"),
        	FOREIGN KEY("VendedorId") REFERENCES "Users"("UserId"),
        	FOREIGN KEY("CompradorId") REFERENCES "Users"("UserId")
        )
        """;
}
