# weather-forecasting

## Requirements:

### Core requirements
Build a weather forecasting REST-service that fetches weather data from a public API and provides responses to clients.
- The service should fetch current weather data from a public API (e.g., OpenWeatherMap, Weatherstack).
- Clients should be able to request weather data for a specific city/date.


### Additional requirements
- Use environment variables to keep the 3rd party weather API key.
- Implement the asynchronous operations.
- Implement error handling with retries and circuit breaking. Respond with user-friendly meaningful error messages. 
- Implement exception handling (dev/prod) with middleware.
- Provide Swagger page.

### Optional requirements

- Introduce authentication API (JWT).
- Provide automated tests for the service (unit / integration).
- Integrate logging (NLog / Serilog), Application Insights.
- Build a simple web client working with the weather forecasting API.
- Prepare the service to be containerized (Docker).
- Build the project using .NET Aspire
- Deploy the weather forecasting service in Azure Cloud as AppService. Provide the script for Azure CLI for deployment or a Bicep file.
- Implement a CI/CD pipeline for the solution with Github Actions(?).
- Implement caching with Redis.
- Decouple the forecast fetching service from the request handling service using a message queue (Azure Service Bus) to handle increased loads.
- Use a database to store: request logs and historical data(?), cache responses(?), ... .


## Guidelines

TBD...

## Notes
