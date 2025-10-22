using GeoBlocker._Models;

namespace GeoBlocker._Services.BlockedCountries
{
    public interface IBlockedCountriesService
    {
        public  Task<ResponseDto> add(string countryCode);
        public  Task<ResponseDto> delete(string countryCode);
        public Task<ResponseDto> getall(string? CountryCodeOrName, int pageIndex, int PageSize,   bool isTempBlocked = false);
        public Task<ResponseDto> addTempBlockedCountry(string? CountrtyCode, int timeInmin);

    }
}
