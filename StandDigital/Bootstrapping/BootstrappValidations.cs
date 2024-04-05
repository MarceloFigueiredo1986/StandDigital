using Microsoft.Extensions.Configuration;

namespace StandDigital.Bootstrapping;

public static class BootstrappValidations
{
    public static void InitialValidations(this IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.GetValue<string>("TelemovelPagamento")))
            throw new ArgumentException("Setting TelemovelPagamento em falta");

        if(configuration.GetSection("Filtros:TiposNegocios").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:TiposNegocios em falta");

        if (configuration.GetSection("Filtros:TipoCombustivel").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:TipoCombustivel em falta");

        if (configuration.GetSection("Filtros:Distritos").Get<string[]>() is null)
            throw new ArgumentException("Setting Filtros:Distritos em falta");
    }
}
