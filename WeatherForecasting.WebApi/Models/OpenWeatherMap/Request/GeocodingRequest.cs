namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record GeocodingRequest(string CityName, string CountryCode = "", string StateCode = "", int Limit = 1) : IQueryParametersRequest
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
		result.Add("limit", Limit.ToString());

		return result;
	}
}
