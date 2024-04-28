namespace StandDigital_Shared.Migrations;

internal class FillScriptTiposNegocios : IScript
{
    public string Script { get; init; } = """
        INSERT OR IGNORE INTO "TipoNegocios" ("TipoNegocioId", "Name")
        VALUES
            (1, 'Leilão'),
            (2, 'Valor Certo'),
            (3, 'Preço incógnita');
        """;
}
