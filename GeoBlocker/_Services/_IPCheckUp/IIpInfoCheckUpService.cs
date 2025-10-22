using GeoBlocker._Models;

namespace GeoBlocker._Services.IPCheckUp
{
    public interface IIpInfoCheckUpService
    {
        public Task<ResponseDto> getCountryInfoById(string? ipAdd= null );
        public Task<ResponseDto> checkIpIfBlocked(string? ip = null);

        public Task<ResponseDto> GetAllBlockedLoggingAttemps(int? pageNo = 1, int? pageSize = 10);
    }
}
