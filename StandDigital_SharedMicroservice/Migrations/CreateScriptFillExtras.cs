using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptFillExtras : IScript
{
    public string Script { get; init; } = """
        INSERT OR IGNORE INTO "Extras" ("ExtraId","Nome")
        VALUES 
        (1,'Ar condicionado'),
        (2,'Sistema de navegação GPS'),
        (3,'Bancos em couro'),
        (4,'Teto solar panorâmico'),
        (5,'Sensores de estacionamento'),
        (6,'Faróis de xenon'),
        (7,'Sistema de som premium'),
        (8,'Cruise Control'),
        (9,'Assistente de faixa de rodagem'),
        (10,'Espelhos retrovisores elétricos'),
        (11,'Bluetooth'),
        (12,'Bancos com regulação elétrica'),
        (13,'Bancos aquecidos'),
        (14,'Assistente de estacionamento'),
        (15,'Retrovisores exteriores aquecidos'),
        (16,'Retrovisores exteriores elétricos'),
        (17,'Assistência ao arranque em subida'),
        (18,'Controlo de tração'),
        (19,'Assistente de pré-colisão'),
        (20,'Faróis de nevoeiro'),
        (21,'Controlo de pressão dos pneus'),
        (22,'Direcção assistida'),
        (23,'Controlo de estabilidade'),
        (24,'Airbag do condutor e passgeiro'),
        (25,'Airbag lateral do condutor e passageiro'),
        (26,'Airbag de coluna');
        """;
}
