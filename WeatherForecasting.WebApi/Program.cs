using WeatherForecasting.WebApi;
using WeatherForecasting.WebApi.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Read the API key from an environment variable
var openWeatherApiKey = Environment.GetEnvironmentVariable("OpenWeatherMap__ApiKey");

// Configure OpenWeatherMapSettings
builder.Services.Configure<OpenWeatherMapSettings>(options =>
{
	options.ApiKey = openWeatherApiKey;
	builder.Configuration.GetSection("OpenWeatherMap").Bind(options);
});

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddWeatherForecastingHttpClient();
builder.Services.AddWeatherStatusHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();