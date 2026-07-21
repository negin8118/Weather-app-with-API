namespace WeatherApp.Models
{
    public class WeatherResultDto
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int Aqi { get; set; }
        public double Pm25 { get; set; }
        public double Pm10 { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Co { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
