using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.Migrations;

internal class CreateScriptFillMarcas : IScript
{
    public string Script { get; init; } = """
        INSERT OR IGNORE INTO "Marcas" ("MarcaId", "Nome", "Image")
        VALUES
            (1, 'Abarth', NULL),
            (2, 'Aiways', NULL),
            (3, 'Alfa Romeo', NULL),
            (4, 'Aston Martin', NULL),
            (5, 'Audi', NULL),
            (6, 'Bentley', NULL),
            (7, 'BMW', NULL),
            (8, 'BYD', NULL),
            (9, 'Citroën', NULL),
            (10, 'Cupra', NULL),
            (11, 'Dacia', NULL),
            (12, 'DS', NULL),
            (13, 'Ferrari', NULL),
            (14, 'Fiat', NULL),
            (15, 'Fisker', NULL),
            (16, 'Ford', NULL),
            (17, 'Honda', NULL),
            (18, 'Hyundai', NULL),
            (19, 'Jaguar', NULL),
            (20, 'Jeep', NULL),
            (21, 'Kia', NULL),
            (22, 'Lamborghini', NULL),
            (23, 'Land Rover', NULL),
            (24, 'Lexus', NULL),
            (25, 'Lotus', NULL),
            (26, 'Maserati', NULL),
            (27, 'Mazda', NULL),
            (28, 'Mercedes-Benz', NULL),
            (29, 'MG', NULL),
            (30, 'MINI', NULL),
            (31, 'Mitsubishi', NULL),
            (32, 'Nissan', NULL),
            (33, 'Opel', NULL),
            (34, 'Peugeot', NULL),
            (35, 'Polestar', NULL),
            (36, 'Porsche', NULL),
            (37, 'Renault', NULL),
            (38, 'SEAT', NULL),
            (39, 'Škoda', NULL),
            (40, 'Smart', NULL),
            (41, 'Suzuki', NULL),
            (42, 'Tesla', NULL),
            (43, 'Toyota', NULL),
            (44, 'Volkswagen', NULL),
            (45, 'Volvo', NULL);
        """;
}
