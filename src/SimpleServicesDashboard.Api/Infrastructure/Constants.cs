using System;

namespace SimpleServicesDashboard.Api.Infrastructure
{
    /// <summary>
    /// Some constants for the API.
    /// </summary>
    public readonly struct Constants
    {
        /// <summary>
        /// Name of the API - to use on OpenAPI specification.
        /// </summary>
        public const string ApiName = "Simple Services Dashboard";

        /// <summary>
        /// Description for the API - to use on OpenAPI specification.
        /// </summary>
        public const string ApiDescription = "Simple Services Dashboard with API";

        /// <summary>
        /// Author of the API.
        /// </summary>
        public const string ApiAuthor = "[Company Name]";

        /// <summary>
        /// Email of teh author (company email).
        /// </summary>
        public const string ApiAuthorEmail = "noreply@companyname.com";

        /// <summary>
        /// Copyright for the Swagger.
        /// </summary>
        public static readonly string Copyright = $"Copyright (c) {DateTime.Today.Year}, [Company Name]";

#pragma warning disable S1075
        /// <summary>
        /// Url to the company main web site.
        /// </summary>
        public const string CompanyUrl = "https://www.companyname.io/";
#pragma warning restore S1075
    }
}