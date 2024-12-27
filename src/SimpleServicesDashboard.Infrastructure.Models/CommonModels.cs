namespace SimpleServicesDashboard.Infrastructure.Models;

/// <summary>
/// Base class for status responses.
/// </summary>
/// <remarks>To remove duplicated code as possible in the logic to collect the data from each service.</remarks>
public class StatusResponseBase
{
    [System.Text.Json.Serialization.JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("appInfo")]
    public AppInfo? AppInfo { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("databaseInfo")]
    public DatabaseInfo? DatabaseInfo { get; set; }
}

/// <summary>
/// Application info.
/// </summary>
public sealed class AppInfo
{
    [System.Text.Json.Serialization.JsonPropertyName("machineName")]
    public string? MachineName { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("environmentName")]
    public string? EnvironmentName { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("releaseDate")]
    public DateTime ReleaseDate { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("appStartTime")]
    public DateTime AppStartTime { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("version")]
    public string? Version { get; set; }
}

/// <summary>
/// Information about database.
/// </summary>
public sealed class DatabaseInfo
{
    [System.Text.Json.Serialization.JsonPropertyName("type")]
    public string? Type { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("connectionString")]
    public string? ConnectionString { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("databaseName")]
    public string? DatabaseName { get; set; }
}