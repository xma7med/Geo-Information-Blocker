using GeoBlocker._Models;
using GeoBlocker._Models.DTOs;
using GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore;
using IPGeolocation;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;

namespace GeoBlocker._Services.IPCheckUp
{
    public class IpInfoCheckUpService : IIpInfoCheckUpService
    {
        private  readonly  HttpClient _http;
        private readonly IBlockedCountryInMemoryStore _blockedCountStore;
        private readonly IHttpContextAccessor _httpCtx;
        private readonly IpAPIoption _options;




        public IpInfoCheckUpService(HttpClient http, IOptions<IpAPIoption> options, IBlockedCountryInMemoryStore blockedCountStore , IHttpContextAccessor httpCtx
            /*IHttpContextAccessor httpContextAccessor*/)
        {
            _http = http;
            _blockedCountStore = blockedCountStore;
            _httpCtx = httpCtx;
            _options = options.Value;
            _http.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
            //_http.BaseAddress=new Uri(_options.BaseUrl);    
            //_httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto> getCountryInfoById (string? ipAdd)
        {
            try
            { 
                #region First way 

                //string Url = $"{_options.BaseUrl}{ipAdd}/json/";
                //HttpResponseMessage response = await _http.GetAsync(Url);
                //response.EnsureSuccessStatusCode();


                //var res = response.Content.ReadFromJsonAsync<GetCountryByIPDto>();
                //return new ResponseDto() { isSuccess = true, data = res, Msg = "Success" }; 

                #endregion

                var obj =  await Peogego(ipAdd);
                if (obj is not null )  
                    return new ResponseDto() { isSuccess = true, data = obj, Msg = obj.Msg };
                else 
                    return new ResponseDto() { data =null , isSuccess = false  , Msg= $"unexpected behavior call backend team , From Peo \n {obj.Msg}" };
                

            }
            catch (Exception ex )
            {
                return new ResponseDto() { isSuccess = false  , Msg = $"unexpected behavior call backend team  , {ex.Message}" };
            }
        }



        public async Task <ResponseDto> checkIpIfBlocked (string? ip = null)
        {
            try
            {
                var allBlockedCount = await _blockedCountStore.getAllBlockedCountry(null);
                var allCountCodes = allBlockedCount.Select(c => c.Code).ToList();

                var CountryInfo = await Peogego(ip);


                var target = CountryInfo.country_code;
                var logging = new LoggingDto();
                logging.ip = CountryInfo.ip;    
                logging.TimeStamp = DateTime.Now;
                logging.CountryCode = CountryInfo.country_code; 
                logging.UserAgent = _httpCtx.HttpContext.Request.Headers.UserAgent .ToString() ?? CountryInfo.org;
               
                if (allCountCodes.Contains(target))
                {
                    logging.BlockedStaus = true; 
                    await _blockedCountStore.addLoggedStamp(logging); 

                    return new ResponseDto() { isSuccess = true, data = true, Msg = "Blocked" };

                }
                else
                {
                    return new ResponseDto() { isSuccess = true, data = false, Msg = "Not Blocked" };

                }

            }
            catch (Exception ex )
            {

                return new ResponseDto() { isSuccess = false, Msg = $"unexpected behavior call backend team  , {ex.Message}" };

            }

        }

        /// Pagenation 
        public async Task <ResponseDto> GetAllBlockedLoggingAttemps (int? pageNo = 1 , int? pageSize =10   )
        {
            try
            {
                var data = await _blockedCountStore.GetallLoggingAttemps();

                var paginatedres = PaginationHelper.ToPagedResult(data , pageNo.Value , pageSize.Value );  

                return new ResponseDto() { isSuccess = true, data = paginatedres, Msg = "Done " };
            }
            catch (Exception ex )
            {

                return new ResponseDto() { isSuccess = false, Msg = $"unexpected behavior call backend team  , {ex.Message}" };

            }


        }


        private async  Task<GetCountryByIPDto> Peogego(string? target = null)
        {
            var obj = new GetCountryByIPDto();
            //IPGeolocationAPI api = new IPGeolocationAPI("add0e291ce96430483152f20ef7db21d");
            IPGeolocationAPI api = new IPGeolocationAPI(_options.ApiKey);
            GeolocationParams geoParams = new GeolocationParams();

            geoParams.SetIPAddress(target); // ---> dev note : uncomment in production enviroment and retest 
            Geolocation geolocation =  api.GetGeolocation(geoParams);

            if (geolocation.GetStatus() == 200)
            {
                obj.country_name = geolocation.GetCountryName();
                obj.org = geolocation.GetOrganization();
                obj.ISP = geolocation.GetISP();
                obj.ip = geolocation.GetIPAddress();



                obj.country_code = geolocation.GetCountryCode2();
                obj.Msg = "retrived Info "; 
                //.GetTimezone().GetCurrentTime());
                return obj;
            }
            obj.Msg = geolocation.GetMessage();
            return obj ;
        }






    }
}
