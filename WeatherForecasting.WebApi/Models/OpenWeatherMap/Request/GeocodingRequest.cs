namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request
{
	public record GeocodingRequest(string CityName, string State, string CountryCode, int Limit) : IQueryParametersRequest
	{
		public Dictionary<string, string> ToQueryParametersDictionary()
		{
			var result = new Dictionary<string, string>();
			var locationQuery = CityName;
			if (!string.IsNullOrWhiteSpace(State) && !string.IsNullOrWhiteSpace(CountryCode))
			{
				locationQuery += $",{State},{CountryCode}";
			}
			else if (!string.IsNullOrWhiteSpace(CountryCode))
			{
				locationQuery += $",{CountryCode}";
			}

			result.Add("q", locationQuery);
			result.Add("limit", Limit.ToString());

			return result;
		}
	}
}
