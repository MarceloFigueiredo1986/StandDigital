using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace StandDigital_Shared.Services.Logging;

public class Logging(ILogger<Logging> logger) : ILogging
{
    private readonly ILogger<Logging> _logger = logger;

    public IDisposable? BeginScope<T>(T inputValues)
        => _logger.BeginScope(JsonSerializer.Serialize(inputValues));

    public void Log(LogLevel logLevel, string Msg, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string? caller = null)
        => _logger.Log(logLevel, Msg, "Line:{lineNumber} Caller method:{caller}", lineNumber, caller);

    public void Log(Exception errorMsg, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string? caller = null)
        => _logger.Log(LogLevel.Error, errorMsg, "Line:{lineNumber} Caller method:{caller}", lineNumber, caller); 
}
