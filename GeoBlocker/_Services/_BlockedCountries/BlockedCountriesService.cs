using GeoBlocker._Models;
using GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore;

namespace GeoBlocker._Services.BlockedCountries
{
    public class BlockedCountriesService : IBlockedCountriesService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IBlockedCountryInMemoryStore _data;

        public BlockedCountriesService(HttpClient httpClient, IConfiguration configuration , IBlockedCountryInMemoryStore data )
        {
            _configuration = configuration;
            _data = data;
            _httpClient = httpClient;
        }

        public async Task<ResponseDto> add (string countryCode)
        {
            try
            {
               bool added = await _data.addBlocked(countryCode);
                if (added)
                    return new ResponseDto() { isSuccess = true , Msg= "Added" , data="signal" };
                return new ResponseDto() { isSuccess = true, Msg = "Duplicated Not Added" };
            }
            catch (Exception ex )
            {
                return new ResponseDto() { isSuccess = false, Msg = $"Smth Happend !! Call Backend Team \n {ex.Message}" };
            }
        }

        public async Task<ResponseDto>  delete(string countryCode)
        {
            try
            {
                bool deleted = await  _data.deleteBlocked(countryCode);
                if (deleted)
                    return new ResponseDto() { isSuccess = true, Msg = "Deleted " };

                return new ResponseDto() { isSuccess = false , Msg = "Not Found" };
            }
            catch (Exception ex)
            {
                return new ResponseDto() { isSuccess = false, Msg = $"Smth Happend !! Call Backend Team \n {ex.Message}" };
            }

        }

        public async Task<ResponseDto> getall (string? CountryCodeOrName , int pageIndex , int PageSize, bool isTempBlocked = false)
        {
            try
            {
                var entities = await _data.getAllBlockedCountry(CountryCodeOrName ,isTempBlocked);

                var res = entities.Select(e => e.Code);

               var pageinatedRes= PaginationHelper.ToPagedResult(res, pageIndex, PageSize);   

                return new ResponseDto() { isSuccess = true,data = pageinatedRes };
            }
            catch (Exception ex  )
            {
                return new ResponseDto() { isSuccess = false, Msg = $"Smth Happend !! Call Backend Team \n {ex.Message}" };
            }

        }


        public async Task <ResponseDto> addTempBlockedCountry (string ? CountrtyCode , int  timeInmin)
        {
            try
            {
                var TimeUtcNow = DateTimeOffset.UtcNow;
                var expTime = TimeUtcNow.AddMinutes(timeInmin);
                ///var obj = new BlockedCountry()
                ///{
                ///    Code = CountrtyCode, 
                ///    ExpiresAfter = expTime,
                ///    IsTemporal = true   
                ///};

                bool res = await _data.addBlocked(CountrtyCode!, true, expTime);

                if (res)
                {
                    return new ResponseDto() { isSuccess = true, Msg = "Added" , data="signal" };

                }
                return new ResponseDto() { isSuccess = true, Msg = "Duplicated Not Added" };
            }
            catch (Exception ex )
            {
                return new ResponseDto() { isSuccess = false, Msg = $"Smth Happend !! Call Backend Team \n {ex.Message}" };
            }

        }








    }
}
