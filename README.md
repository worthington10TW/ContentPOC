# About
This is a project that has been created using the LN micro-service template.

The template includes the following top-level entities:
* `gocd` - pipeline for building the service using GoCD
* `ContentPOC` - the application source
* `ContentPOC.Test` - the application unit tests
* `ContentPOC.Integration` - the application integration tests
* `Dockerfile` - for generating a Docker container wrapping the application

# Usage

## Run the micro-service
1. ```cd <MyProjectName>```
1. ```dotnet run --project <MyProjectName>```

## Available endpoints
The following endpoints are exported from the application:

### https://localhost:5000
* `/` - redirects to /swagger
* `/api/v1/greeting` - returns a JSON greeting (HomeController)
* `/api/v2/greeting` - returns a textual greeting (HomeController2)
* `/swagger` - returns UI for API documentation (Swashbuckle middleware)
* `/swagger/v1/swagger.json` - returns the API documentation for v1 controllers in JSON format (Swashbuckle middleware)
* `/swagger/v2/swagger.json` - returns the API documentation for v2 controllers in JSON format (Swashbuckle middleware)

### http://localhost:5001
* `/health` - returns health information ([AppMetrics](https://www.app-metrics.io/web-monitoring/aspnet-core/))
* `/metrics` - returns metrics details ([AppMetrics](https://www.app-metrics.io/web-monitoring/aspnet-core/))

## Adding API Versions
To add more versions to your API, or to extend the current behaviour see https://github.com/Microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path.
Swagger endpoints will automatically be added for each version defined.

## Versioning your Microservice
This can be found as properties in the main `ContentPOC.csproj`

