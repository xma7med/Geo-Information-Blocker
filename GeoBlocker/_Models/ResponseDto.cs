namespace GeoBlocker._Models
{
    public class ResponseDto
    {
        public ResponseDto(bool isSuccess = true, object? data = null, string? Msg = null)
        {
            this.isSuccess = isSuccess;
            this.data = data;
            this.Msg = Msg;
        }
        public bool isSuccess { get; set; } = true;


        public object? data { get; set; }
        public string? Msg { get; set; }
        //public int? StatusCode  { get; set; }

    }
}
