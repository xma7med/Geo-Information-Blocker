namespace GeoBlocker._Models
{
    public class IpAPIoption
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 15;
    }
}
