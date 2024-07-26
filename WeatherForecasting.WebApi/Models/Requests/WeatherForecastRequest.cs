namespace WeatherForecasting.WebApi.Models.Requests
{
	public record WeatherForecastRequest(decimal Latitude, decimal Longitude, int ForecastsCount) : BaseWeatherRequest(Latitude, Longitude)
	{
		public override Dictionary<string, string> ToQueryParametersDictionary()
		{
			var result = base.ToQueryParametersDictionary();
			//result.Add("cnt", ForecastsCount.ToString());

			return result;
		}
	}
}
