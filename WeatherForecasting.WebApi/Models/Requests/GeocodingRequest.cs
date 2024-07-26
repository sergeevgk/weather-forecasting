namespace WeatherForecasting.WebApi.Models.Requests
{
	public record GeocodingRequest(string CityName, string StateCode, string CountryCode) : IQueryParametersRequest
	{
		public Dictionary<string, string> ToQueryParametersDictionary()
		{
			var result = new Dictionary<string, string>();
			var locationQuery = CityName;
			if (!string.IsNullOrWhiteSpace(StateCode) && !string.IsNullOrWhiteSpace(CountryCode))
			{
				locationQuery += $",{StateCode},{CountryCode}";
			}
			else if (!string.IsNullOrWhiteSpace(CountryCode))
			{
				locationQuery += $",{CountryCode}";
			}

			result.Add("q", locationQuery);

			return result;
		}
	}
}
