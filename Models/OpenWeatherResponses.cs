namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        public Coord Coord { get; set; }
        public MainInfo Main { get; set; }
        public WindInfo Wind { get; set; }
    }

    public class Coord { public double Lon { get; set; } public double Lat { get; set; } }
    public class MainInfo { public double Temp { get; set; } public int Humidity { get; set; } }
    public class WindInfo { public double Speed { get; set; } }

    public class AirQualityResponse
    {
        public List<AirItem> List { get; set; }
    }

    public class AirItem
    {
        public AirMain Main { get; set; }
        public Components Components { get; set; }
    }

    public class AirMain { public int Aqi { get; set; } }

    public class Components
    {
        public double Pm25 { get; set; }
        public double Pm10 { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double Co { get; set; }
    }

}
