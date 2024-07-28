# weather-forecasting

## Project description: 
A small WebAPI for weather forecasting and current weather information built with .NET 8, .NET Aspire, Redis. 
Due to the free Weather API limitations at the moment supports only 3-hour forecasts for 5 next days. The query rate limit is 60 per minute due to the same reason.

### Setup:
1. Clone the repository
2. Provide a [OpenWeatherMap API key](https://openweathermap.org/appid) as an Environment Variable `OpenWeatherMap__ApiKey` or as a corresponding configuration in appsettings.Development.json.
3. Provide a "cache" connection string to a Redis server in the `ConnectionStrings` configuration section. Redis can be hosted in WSL2 in Windows, see https://redis.io/docs/latest/operate/oss_and_stack/install/install-redis/install-redis-on-windows/.
4. Run the project `WeatherForecasting.AppHost` via Visual Studio or via dotnet command line (`dotnet run`). 
	At the moment there is no difference from running the `WatherForecasting.WebApi` project - Aspire just provides logs in a more convenient format. Later there will be Redis hosting and, hopefully, many other features. 
5. Swagger documentation and interface are provided at {running_app_url}/swagger.

### Requirements and features:

#### Implemented
- The service should fetch current weather data from a public API (e.g., OpenWeatherMap, Weatherstack).
- Clients should be able to request weather data for a specific city/date.
- Use environment variables to keep the 3rd party weather API key.
- Implement the asynchronous operations.
- Implement exception handling with middleware. Respond with user-friendly meaningful error messages.
- Provide Swagger page.
- Provide automated tests for the service (unit / integration).
- Integrates logging (built-in logger), Application Insights (integration via Aspire, need to populated with data in WebAPI).
- Implement caching with Redis.


#### Planned
- Implement error handling with retries.
- Enhance automated testing, provide more valuable test cases, implement integration tests.
- Prepare the service to be containerized (Docker/Podman).
- Decouple the forecast fetching service from the request handling service using a message queue (Azure Service Bus) to handle increased loads.
- Introduce authentication API (JWT) for the service. Maybe add some user management?
- Create a simple UI client, try to publish the API and the client in Azure. Deploy the weather forecasting service in Azure Cloud as AppService. Provide the script for Azure CLI for deployment or a Bicep file.
- Implement a CI/CD pipeline for the solution with Github Actions or Azure DevOps.


#### Under consideration
- Build something using Event Sourcing paradigm. 
For example: query 3rd party API (some scheduled operation), create a WeatherStateUpdate command and validate it, then publish CurrentWeatherStateUpdate event.
Append the event to the log, in addition populate Redis cache or some document DB with the updated data.
Think about some OLAP queries to run against the database with possibly a different model.