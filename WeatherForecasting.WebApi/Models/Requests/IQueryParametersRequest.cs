namespace WeatherForecasting.WebApi.Models.Requests
{
	public interface IQueryParametersRequest
	{
		public Dictionary<string, string> ToQueryParametersDictionary();
	}
}
