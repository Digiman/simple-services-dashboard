using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SimpleServicesDashboard.Common.Configuration;

/// <summary>
/// Configuration for Health Checks and UI.
/// </summary>
public sealed class HealthCheckOptions
{
    [Required]
    public bool HealthCheckUiEnabled { get; set; }
    [Required]
    public required string HeaderText { get; set; }
    public int EvaluationTimeInSeconds { get; set; }
    public int MaximumHistoryEntriesPerEndpoint { get; set; }
}

/// <summary>
/// Custom validator for HealthCheckOptions with FluentValidator.
/// </summary>
public sealed class HealthCheckOptionsValidator : AbstractValidator<HealthCheckOptions>
{
    public HealthCheckOptionsValidator()
    {
        RuleFor(x => x.HealthCheckUiEnabled).NotEmpty();
        RuleFor(x => x.HeaderText).NotEmpty();
    }
}