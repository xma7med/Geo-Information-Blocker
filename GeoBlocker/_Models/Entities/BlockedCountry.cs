namespace GeoBlocker._Models.Entities
{
    public class BlockedCountry
    {
        public BlockedCountry(string? code=null  )
        {
            Code = code;    
        }
        public string Code { get; set; }


        public string? Name { get; init; }
        public bool? IsTemporal { get; init; } = false; // true لو حجب مؤقت
        public DateTimeOffset? ExpiresAfter { get; init; } = null;
        //public DateTime TimeStamp { get; set; }

    }
}
