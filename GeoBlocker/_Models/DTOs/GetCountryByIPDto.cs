namespace GeoBlocker._Models.DTOs
{
    public class GetCountryByIPDto
    {
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string org { get; set; }
        public string ISP { get; set; }

        public string? Msg { get; set; } = null; 
    }
}
