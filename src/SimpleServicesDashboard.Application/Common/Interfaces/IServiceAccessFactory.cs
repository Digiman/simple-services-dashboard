namespace SimpleServicesDashboard.Application.Common.Interfaces
{
    /// <summary>
    /// Simple factory to create needed instance of the service access class instance to access to the service via API.
    /// </summary>
    public interface IServiceAccessFactory
    {
        /// <summary>
        /// Get service access instance to process he requests to the service API.
        /// </summary>
        /// <param name="serviceCode">Service code (as in configuration).</param>
        /// <returns>Returns instance of the service access class.</returns>
        IServiceAccess GetServiceAccess(string serviceCode);
    }
}