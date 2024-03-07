namespace StandDigital.Core.Entities;

public class UserEntity
{
    public required int UserId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string NIF { get; set; }
    public int NIFValidated { get; set; }
    public required string Email { get; set; }
    public required string Mobile { get; set; }
    public required string Username { get; set; }
    public int EmailValidated { get; set; }
    public required string Password { get; set; }
    public int Ativo { get; set; }
    public required string Role { get; set; }
}
