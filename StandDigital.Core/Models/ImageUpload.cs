namespace StandDigital.Core.Models;

public class ImageUpload
{
    public required string AbsoluteNameWithPath { get; set; }
    public required string ImageName { get; set; }
    public bool MainImage { get; set; }
}
