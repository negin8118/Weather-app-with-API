using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public WeatherService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<WeatherResultDto?> GetCityWeatherAsync(string city)
        {

            var apiKey = _config["OpenCityWeather:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return null;


            var weatherUrl =
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            var weatherResponse = await _http.GetAsync(weatherUrl);

            if (weatherResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            weatherResponse.EnsureSuccessStatusCode();

            var weatherResp = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
            if (weatherResp == null || weatherResp.Coord == null)
            {
                return null;
            }



            if (weatherResp == null ||
                weatherResp.Coord == null ||
                weatherResp.Main == null ||
                weatherResp.Wind == null)
            {
                return null;
            }

            var lat = weatherResp.Coord.Lat;
            var lon = weatherResp.Coord.Lon;


            var airUrl =
                $"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={apiKey}";

            var airResp =
                await _http.GetFromJsonAsync<AirQualityResponse>(airUrl);


            if (airResp == null ||
                airResp.List == null ||
                !airResp.List.Any() ||
                airResp.List.First().Main == null ||
                airResp.List.First().Components == null)
            {
                return null;
            }

            var air = airResp.List.First();


            return new WeatherResultDto
            {
                City = city,
                Temperature = weatherResp.Main.Temp,
                Humidity = weatherResp.Main.Humidity,
                WindSpeed = weatherResp.Wind.Speed,
                Aqi = air.Main.Aqi,
                Pm25 = air.Components.Pm25,
                Pm10 = air.Components.Pm10,
                No2 = air.Components.No2,
                O3 = air.Components.O3,
                Co = air.Components.Co,
                Latitude = lat,
                Longitude = lon
            };
        }



    }
}