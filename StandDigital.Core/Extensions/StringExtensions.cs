namespace StandDigital.Core.Extensions;

public static class StringExtensions
{
    public static string? ImageStringBase64(this byte[] image)
        => image is not null ? $"data:image/png;base64, {Convert.ToBase64String(image)}" : null;
}
