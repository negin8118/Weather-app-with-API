namespace WeatherApp.Tests;
using Xunit;
using WeatherApp.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;




public class WeatherServiceTests
{
    [Fact]
    public async Task GetCityWeatherAsync_ReturnsNull_WhenApiReturnsEmptyJson()
    {
        
        var fakeHandler = new FakeHttpHandler("{}");
        var httpClient = new HttpClient(fakeHandler);

        
        var config = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string> { { "OpenCityWeather:ApiKey", "test" } }
        ).Build();

        var service = new WeatherService(httpClient, config);

        
        var result = await service.GetCityWeatherAsync("InvalidCity");

        
        Assert.Null(result);
    }

    public class FakeSequenceHandler : HttpMessageHandler
    {
        private readonly Queue<string> _responses;

        public FakeSequenceHandler(params string[] responses)
        {
            _responses = new Queue<string>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var json = _responses.Dequeue();

            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json)
                });
        }
    }


    [Fact]
    public async Task GetCityWeatherAsync_ReturnsDto_WhenDataIsValid()
    {
        
        var fakeWeatherJson = @"{
        ""coord"": { ""lat"": 35.7, ""lon"": 51.4 },
        ""main"": { ""temp"": 22.5, ""humidity"": 55 },
        ""wind"": { ""speed"": 4.2 }
    }";

        
        var fakeAirJson = @"{
        ""list"": [
            {
                ""main"": { ""aqi"": 3 },
            ""components"": {
                    ""pm25"": 12.5,
                    ""pm10"": 30.2,
                    ""no2"": 15.3,
                    ""o3"": 25.8,
                    ""co"": 0.4
                }
            }
        ]
    }";

        var fakeHandler = new FakeSequenceHandler(fakeWeatherJson, fakeAirJson);
        var httpClient = new HttpClient(fakeHandler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string> { { "OpenCityWeather:ApiKey", "test" } }
            ).Build();

        var service = new WeatherService(httpClient, config);

        
        var result = await service.GetCityWeatherAsync("Tehran");

        
        Assert.NotNull(result);
        Assert.Equal("Tehran", result.City);
        Assert.Equal(22.5, result.Temperature);
        Assert.Equal(55, result.Humidity);
        Assert.Equal(4.2, result.WindSpeed);
        Assert.Equal(3, result.Aqi);
        Assert.Equal(12.5, result.Pm25);
        Assert.Equal(30.2, result.Pm10);
        Assert.Equal(15.3, result.No2);
        Assert.Equal(25.8, result.O3);
        Assert.Equal(0.4, result.Co);
        Assert.Equal(35.7, result.Latitude);
        Assert.Equal(51.4, result.Longitude);
    }


}

public class FakeHttpHandler : HttpMessageHandler
{
    private readonly string _response;

    public FakeHttpHandler(string response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var msg = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_response)
        };

        return Task.FromResult(msg);
    }
}
