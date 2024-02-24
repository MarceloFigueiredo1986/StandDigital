namespace StandDigital.Models;

public class ImageUpload
{
    public required string AbsoluteName { get; set; }
    public required string ImageName { get; set; }
    public bool MainImage { get; set; }
    public byte[] ImageBa { get; set; }
}
