using System.Diagnostics.Metrics;

namespace GeoBlocker._Models.DTOs
{
    public class LoggingDto
    {
        public string  ip { get; set; }
        public DateTime? TimeStamp { get; set; } = null;
        public string  CountryCode { get; set; }
        public bool  BlockedStaus { get; set; } // 
        public string UserAgent { get; set; } // company 

    }
}
