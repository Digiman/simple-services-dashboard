namespace SimpleServicesDashboard.Common.Configuration
{
    /// <summary>
    /// Configuration for Health Checks and UI.
    /// </summary>
    public sealed class HealthCheckOptions
    {
        public bool HealthCheckUiEnabled { get; set; }
        public string HeaderText { get; set; }
        public int EvaluationTimeInSeconds { get; set; }
        public int MaximumHistoryEntriesPerEndpoint { get; set; }
    }
}