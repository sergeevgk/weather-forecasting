var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var apiService = builder.AddProject<Projects.WeatherForecasting_WebApi>("apiservice")
						.WithReference(cache);

builder.Build().Run();
