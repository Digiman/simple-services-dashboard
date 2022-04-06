using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimpleServicesDashboard.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleServicesDashboard.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure application custom configuration.
        /// </summary>
        /// <param name="services">Services collection.</param>
        public static void AddConfiguration(this IServiceCollection services)
        {
            services.AddApplicationOptions<HealthCheckOptions>(nameof(HealthCheckOptions));
            services.AddApplicationOptions<ServicesConfigurationOptions>(nameof(ServicesConfigurationOptions));
        }

        /// <summary>
        /// Configures and validates options.
        /// </summary>
        /// <typeparam name="TOptions">The options type that should be added.</typeparam>
        /// <param name="services">The dependency injection container to add options.</param>
        /// <param name="key">
        /// The configuration key that should be used when configuring the options.
        /// If null, the root configuration will be used to configure the options.
        /// </param>
        /// <returns>The dependency injection container.</returns>
        private static IServiceCollection AddApplicationOptions<TOptions>(
            this IServiceCollection services,
            string key = null)
            where TOptions : class
        {
            services.AddSingleton<IValidateOptions<TOptions>>(new ValidateServiceOptions<TOptions>(key));
            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                if (key != null)
                {
                    config = config.GetSection(key);
                }

                return new BindOptions<TOptions>(config);
            });

            return services;
        }
    }

    /// <summary>
    /// A configuration that validates options using data annotations.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to validate.</typeparam>
    public sealed class ValidateServiceOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly string _optionsName;

        /// <summary>
        /// Create a new validator.
        /// </summary>
        /// <param name="optionsName">
        /// The option's key in the configuration or appsettings.json file,
        /// or null if the options was created from the root configuration.
        /// </param>
        public ValidateServiceOptions(string optionsName)
        {
            _optionsName = optionsName;
        }

        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            var context = new ValidationContext(options);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(options, context, validationResults, validateAllProperties: true))
            {
                return ValidateOptionsResult.Success;
            }

            var errors = new List<string>();
            var message = (_optionsName == null)
                ? $"Invalid configs"
                : $"Invalid '{_optionsName}' configs";

            foreach (var result in validationResults)
            {
                errors.Add($"{message}: {result}");
            }

            return ValidateOptionsResult.Fail(errors);
        }
    }

    /// <summary>
    /// Automatically binds configs to options.
    /// </summary>
    /// <typeparam name="TOptions">The options to bind to.</typeparam>
    public class BindOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Automatically bind these configurations to the options.
        /// </summary>
        /// <param name="config">The configs to automatically bind to options.</param>
        public BindOptions(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Configure(TOptions options) => _config.Bind(options);
    }
}