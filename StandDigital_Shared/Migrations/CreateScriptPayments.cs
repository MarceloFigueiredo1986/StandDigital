
namespace StandDigital_Shared.Migrations;

internal class CreateScriptPayments : IScript
{
    public string Script { get; init; } = """
                CREATE TABLE IF NOT EXISTS "Pagamentos" (
        	"PagamentoId"	INTEGER NOT NULL UNIQUE,
        	"CarroId"	INTEGER NOT NULL,
        	"TokenPagamento"	TEXT NOT NULL,
        	"DataCriacao"	TEXT NOT NULL,
        	FOREIGN KEY("CarroId") REFERENCES "CarrosFotos"("CarroId"),
        	PRIMARY KEY("PagamentoId" AUTOINCREMENT)
        )
        """;
}