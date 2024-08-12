var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var apiService = builder.AddProject<Projects.WeatherForecasting_WebApi>("apiservice")
						.WithReference(cache)
						.WithExternalHttpEndpoints();

var angularClient = builder.AddNpmApp("AngularFrontEnd", "../frontend-clients/weather-angular-client")
						.WithReference(apiService)
						.WithHttpEndpoint(port: 4200, env: "PORT", name: "http")
						.WithExternalHttpEndpoints()
						.PublishAsDockerFile();

builder.Build().Run();
