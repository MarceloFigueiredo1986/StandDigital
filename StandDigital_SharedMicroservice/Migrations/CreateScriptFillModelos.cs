using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptFillModelos : IScript
{
    public string Script { get; init; } = """
        INSERT OR IGNORE INTO "Modelos" ("Nome", "Cavalagem", "MarcaId", "Combustivel", "Segmento")
        VALUES
            ('500', 160, 1, 'Gasolina', 'Esportivo'),
            ('Giulia', 280, 3, 'Gasolina', 'Sedan'),
            ('Stelvio', 280, 3, 'Gasolina', 'SUV'),
            ('A3', 150, 5, 'Diesel', 'Hatchback'),
            ('Q5', 190, 5, 'Gasolina', 'SUV'),
            ('320i', 184, 7, 'Gasolina', 'Sedan'),
            ('X3', 248, 7, 'Diesel', 'SUV'),
            ('C3', 110, 9, 'Gasolina', 'Hatchback'),
            ('C5 Aircross', 130, 9, 'Diesel', 'SUV'),
            ('Sandero', 90, 11, 'Gasolina', 'Hatchback'),
            ('Duster', 115, 11, 'Diesel', 'SUV'),
            ('500', 69, 14, 'Gasolina', 'Hatchback'),
            ('Tipo', 95, 14, 'Diesel', 'Sedan'),
            ('Focus', 125, 16, 'Gasolina', 'Hatchback'),
            ('Kuga', 150, 16, 'Híbrido', 'SUV'),
            ('XE', 250, 19, 'Gasolina', 'Sedan'),
            ('F-PACE', 300, 19, 'Diesel', 'SUV'),
            ('Renegade', 120, 20, 'Gasolina', 'SUV'),
            ('Compass', 170, 20, 'Diesel', 'SUV'),
            ('Ceed', 140, 21, 'Gasolina', 'Hatchback'),
            ('Sportage', 185, 21, 'Diesel', 'SUV'),
            ('Range Rover Evoque', 200, 23, 'Diesel', 'SUV'),
            ('Discovery Sport', 240, 23, 'Híbrido', 'SUV'),
            ('Mazda3', 122, 27, 'Gasolina', 'Hatchback'),
            ('CX-5', 165, 27, 'Diesel', 'SUV'),
            ('Classe A', 163, 28, 'Gasolina', 'Hatchback'),
            ('GLC', 194, 28,'Gasolina','SUV')
        """;
}
