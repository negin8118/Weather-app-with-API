# Weather & Air Quality API

An ASP.NET Core Web API that returns current weather and air quality data 
for a given city, using the OpenWeatherMap API.

## Features
- Takes a city name as input
- Returns temperature, humidity, and wind speed
- Returns air quality index (AQI) and pollutant levels (PM2.5, PM10, NO2, O3, CO)
- Returns the city's latitude and longitude
- Returns `null` for invalid/unrecognized city names
- Swagger UI for interactive testing
- Unit tests (xUnit) covering both valid and invalid city scenarios

## Tech Stack
- ASP.NET Core Web API (.NET 8)
- OpenWeatherMap API (Weather + Air Pollution endpoints)
- xUnit for unit testing
- Swagger / OpenAPI


Tests cover:
- Returning `null` when the API response is empty (invalid city)
- Returning correct weather + air quality data when the response is valid
