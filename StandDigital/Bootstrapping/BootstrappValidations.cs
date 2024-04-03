using Microsoft.Extensions.Configuration;

namespace StandDigital.Bootstrapping;

public static class BootstrappValidations
{
    public static void InitialValidations(this IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.GetValue<string>("TelemovelPagamento")))
            throw new ArgumentException("Setting TelemovelPagamento em falta");
    }
}
