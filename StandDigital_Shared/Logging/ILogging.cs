using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace StandDigital_Shared.Services.Logging;

public interface ILogging
{
    void Log(LogLevel logLevel, string Msg, [CallerLineNumber] int lineNumber = 0,
    [CallerMemberName] string? caller = null);
    void Log(Exception errorMsg, [CallerLineNumber] int lineNumber = 0,
    [CallerMemberName] string? caller = null);

    IDisposable? BeginScope<T>(T inputValues);
}
