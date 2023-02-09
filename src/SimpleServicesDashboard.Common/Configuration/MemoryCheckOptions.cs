namespace SimpleServicesDashboard.Common.Configuration;

/// <summary>
/// Options for the simple memory check.
/// </summary>
public sealed class MemoryCheckOptions
{
    /// <summary>
    /// Failure threshold (in bytes) - default 256 Mb.
    /// </summary>
    private const long DefaultThreshold = 1024L * 1024L * 256L;

    /// <summary>
    /// Failure threshold (in bytes).
    /// </summary>
    public long Threshold { get; set; } = DefaultThreshold;
}