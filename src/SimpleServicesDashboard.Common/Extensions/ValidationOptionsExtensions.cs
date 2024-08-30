using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace SimpleServicesDashboard.Common.Extensions;

// See more: https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-using-flentvalidation/

/// <summary>
/// Extensions for configuration for Validation config sections.
/// </summary>
public static class ValidationOptionsExtensions
{
    /// <summary>
    /// Configure options with validation by default DataAnnotations.
    /// </summary>
    /// <param name="services">Services collections.</param>
    /// <param name="configurationSection">Name of teh configuration section in file.</param>
    /// <typeparam name="TOptions">Type of the options to be configured.</typeparam>
    /// <returns>Returns updated services collection.</returns>
    public static OptionsBuilder<TOptions> AddWithValidation<TOptions>(
        this IServiceCollection services,
        string configurationSection)
        where TOptions : class
    {
        return services.AddOptions<TOptions>()
            .BindConfiguration(configurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    /// <summary>
    /// Configure options with validation by using FluentValidation.
    /// </summary>
    /// <param name="services">Services collections.</param>
    /// <param name="configurationSection">Name of teh configuration section in file.</param>
    /// <typeparam name="TOptions">Type of the options to be configured.</typeparam>
    /// <typeparam name="TValidator">Type of the validator class </typeparam>
    /// <returns>Returns updated services collection.</returns>
    public static OptionsBuilder<TOptions> AddWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        string configurationSection)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
    {
        // Add the validator
        services.AddScoped<IValidator<TOptions>, TValidator>();

        return services.AddOptions<TOptions>()
            .BindConfiguration(configurationSection)
            .ValidateFluentValidation()
            .ValidateOnStart();
    }
}

public static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            provider => new FluentValidationOptions<TOptions>(optionsBuilder.Name, provider));
        return optionsBuilder;
    }
}

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _name;

    public FluentValidationOptions(string? name, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _name = name;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (_name != null && _name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        // Ensure options are provided to validate against
        ArgumentNullException.ThrowIfNull(options);

        // Validators are registered as scoped, so need to create a scope,
        // as we will be called from the root scope
        using var scope = _serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var results = validator.Validate(options);
        if (results.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        string typeName = options.GetType().Name;
        var errors = new List<string>();
        foreach (var result in results.Errors)
        {
            errors.Add($"Fluent validation failed for '{typeName}.{result.PropertyName}' with the error: '{result.ErrorMessage}'.");
        }

        return ValidateOptionsResult.Fail(errors);
    }
}