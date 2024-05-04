namespace StandDigital_Shared.Migrations;

public interface IMigration
{
    Task RunMigrationsAsync();
}
