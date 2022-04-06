using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.Custom.Enrichers
{
    /// <summary>
    /// Enriches log events with an EC2InstanceId property containing the current EC2 instance Id.
    /// </summary>
    public class Ec2InstanceIdEnricher : ILogEventEnricher
    {
        private LogEventProperty _cachedProperty;

        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string Ec2InstanceIdPropertyName = "EC2InstanceId";

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var instanceId = Amazon.Util.EC2InstanceMetadata.InstanceId;
            if (!string.IsNullOrEmpty(instanceId))
            {
                _cachedProperty ??= propertyFactory.CreateProperty(Ec2InstanceIdPropertyName, instanceId);

                logEvent.AddPropertyIfAbsent(_cachedProperty);
            }
        }
    }
}