using Microsoft.Extensions.Logging;

namespace SimpleServicesDashboard.Common.Extensions;

/// <summary>
/// Extensions for logger.
/// </summary>
public static class LoggerExtensions
{
    public static IDisposable? BeginNamedScope(this ILogger logger,
        string name, params ValueTuple<string, object>[] properties)
    {
        var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);
        dictionary[name + ".Scope"] = Guid.NewGuid();
        return logger.BeginScope(dictionary);
    }

    public static IDisposable? BeginPropertyScope(this ILogger logger,
        params ValueTuple<string, object>[] properties)
    {
        var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);
        return logger.BeginScope(dictionary);
    }
}