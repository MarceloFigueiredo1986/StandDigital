using System.Collections.Frozen;

namespace StandDigital.Settings;

public class AppSettings
{
    public string LogoPath { get; init; }
    public string CDNbaseUri { get; init; }
    public FrozenSet<string> TiposNegocios { get; init; }
    public FrozenSet<string> Distritos { get; init; }
    public FrozenSet<string> TiposCombustivel { get; init; }
    public FrozenDictionary<string,string> Segmentos { get; init; }

    public AppSettings(IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.GetValue<string>("TelemovelPagamento")))
            throw new ArgumentException("Setting TelemovelPagamento em falta");

        if (configuration.GetSection("Filtros:TiposNegocios").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:TiposNegocios em falta");

        if (configuration.GetSection("Filtros:TiposCombustivel").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:TipoCombustivel em falta");

        if (configuration.GetSection("Filtros:Distritos").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:Distritos em falta");

        if (!configuration.GetSection("Filtros:Segmentos").GetChildren().Any())
            throw new ArgumentException("Setting Filtros:Segmentos em falta");


        LogoPath = configuration.GetValue<string>("Paths:LogoPath");
        CDNbaseUri = configuration.GetValue<string>("CDNbaseUri");
        TiposNegocios = configuration.GetSection("Filtros:TiposNegocios").Get<string[]>()!.ToFrozenSet();
        TiposCombustivel = configuration.GetSection("Filtros:TiposCombustivel").Get<string[]>()!.ToFrozenSet();
        Distritos = configuration.GetSection("Filtros:Distritos").Get<string[]>()!.ToFrozenSet();
        Segmentos = configuration.
            GetSection("Filtros:Segmentos").
            GetChildren().
            Select(a => new { a.GetChildren().First().Key, a.GetChildren().First().Value }).
            ToFrozenDictionary(a => a.Key,a => a.Value)!;
    }
}