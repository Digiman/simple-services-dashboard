using System.ComponentModel.DataAnnotations;

namespace SimpleServicesDashboard.Common.Configuration
{
    /// <summary>
    /// Configuration for Health Checks and UI.
    /// </summary>
    public sealed class HealthCheckOptions
    {
        [Required]
        public bool HealthCheckUiEnabled { get; set; }
        [Required]
        public string HeaderText { get; set; }
        public int EvaluationTimeInSeconds { get; set; }
        public int MaximumHistoryEntriesPerEndpoint { get; set; }
    }
}