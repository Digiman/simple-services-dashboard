using System.Reflection;
using FluentValidation;
using SimpleServicesDashboard.Application.Common.Behaviours;
using SimpleServicesDashboard.Application.Services;
using SimpleServicesDashboard.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleServicesDashboard.Application
{
    /// <summary>
    /// Dependency registrator for Application stuff.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register application level dependencies and services.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>Returns updated services collection.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // register mapping profiles for AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // register Validators (FluentValidation)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // register MediatR stuff
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            // register application services
            services.AddTransient<IApplicationStatusService, ApplicationStatusService>();
            services.AddTransient<IServicesStatusService, ServicesStatusService>();

            return services;
        }
    }
}