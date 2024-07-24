var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WeatherForecasting_WebApi>("apiservice");

builder.Build().Run();
