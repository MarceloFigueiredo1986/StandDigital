using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptFillExtras : IScript
{
    public string Script { get; init; } = """
        INSERT OR IGNORE INTO "Extras" ("Nome")
        VALUES ('Ar condicionado');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Sistema de navegação GPS');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Bancos em couro');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Teto solar panorâmico');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Sensores de estacionamento');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Faróis de xenon');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Sistema de som premium');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Cruise Control');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Assistente de faixa de rodagem');

        INSERT INTO "Extras" ("Nome")
        VALUES ('Espelhos retrovisores elétricos');
        """;
}
