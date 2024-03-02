namespace StandDigital.Core.Entities;

public class UserEntity
{
    public required int UserId { get; set; }
    public required string Name { get; set; }
    public string StandName { get; set; }
    public string Address { get; set; }
    public string NIF { get; set; }
    public int NIFValidated { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Username { get; set; }
    public int EmailValidated { get; set; }
    public string Password { get; set; }
    public int Ativo { get; set; }
    public string Role { get; set; }
}
