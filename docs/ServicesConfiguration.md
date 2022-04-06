# Services Configuration to monitor

This page provided information about configuration foe the service to monitor other services. This is very simple and useful to add needed config
to monitor even local services during development.

## Configuration

Configuration for the services to monitor and also to configure table with dashboard contains 2 parts:

* Environments - list of environments definitions to use.
    * Reflects on the columns in the table to show the service details.
* Services - list of details for each service.
    * Included service details and list of environments for each specific service.

> *Currently the service used this configuration because it’s enough and could provided flexibility required for the application at this moment.
Maybe in future it can be extended to support additional settings.*

### Environments section

This is array of the objects. Each object need to contains next values:
Name - full name of the environment - use to show in the table header. Information field.
Code - short code used as a key inside the application to select proper settings for each environment for the service.
Sample section:

```json
"Environments" : [
    {
        "Code": "dev",
        "Name": "Development"
    },
    {
        "Code": "qa",
        "Name": "QA"
    },
    {
        "Code": "uat",
        "Name": "UAT"
    },
    {
        "Code": "prod",
        "Name": "Production"
    }
]
```

This section provided the list of environments to monitor. Based on the application environment it can manage the table columns with the
services dashboard. It works dynamically so we can add more environment here and see the changes in the table.

### Services section

Section provided configurations for each service to monitor. Here we need to provided the objects with properties:

1. Code - service code - short value to use to get service details and select proper logic to get service status inside the application.
2. Name - full service name - used to show the service name in Health Check dashboard
3. HealthEndpoint - service internal endpoint for health checks - all the services.
4. AboutEndpoint - service internal endpoint to et application status - used to get information about service for dashboard (version, dates,
etc.) - common solution that provided in service template and customized for each service depending on the internals.
5. Environments:
    * Environment - environment code to match the code with code in Environments section.
    * BaseUrl - the service address to call - will be used as base path for specific environment to call About and Health endpoints.

> *The configuration for endpoints is shared for all the services because all the environment will, be the same in scope of the logic and
endpoints. So consideration - the same code.*

Service configuration sample:

```json
{
    "Code": "code",
    "Name": "SampleService",
    "HealthEndpoint": "/health",
    "AboutEndpoint": "/api/v1/status",
    "Environments": [
        {
            "Environment": "dev",
            "BaseUrl": "https://service-url-dev.net"
        },
        {
            "Environment": "qa",
            "BaseUrl": "https://service-url-qa.net"
        },
        {
            "Environment": "uat",
            "BaseUrl": "https://service-url-uat.net"
        },
        {
            "Environment": "prod",
            "BaseUrl": "https://service-url-prod.net"
        }
    ]
}
```

In this sample provided one service with 4 environments. So it means that in table on dashboard we will see details for service for each
environment.

There are possible behavior:

1.  If the service is not responding (ore return an error) - message Service is unavailable! will be shown.
2. If environment is not provided (for one from the available list) - message No service details! will be shown.
It’s very simple but useful to have single place to see the services version and other details.
