using GeoBlocker._Services.IPCheckUp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GeoBlocker._Controllers
{
    
    public class ipController:SharedController
    {
        private readonly IHttpContextAccessor _http;
        private readonly IIpInfoCheckUpService _ipInfoCheck;

        public ipController(IHttpContextAccessor http ,IIpInfoCheckUpService ipInfoCheck )
        {
            _http = http;
            _ipInfoCheck = ipInfoCheck;
        }


        [HttpGet("lookup")]
        public async Task<IActionResult> GetCountryInfoByIp ([FromQuery] string ? ipAddress = null )
        {
            if (ipAddress != null && !IsValidIp(ipAddress)) return BadRequest("Invalid IP ");
           if (ipAddress == null) ipAddress = extractIp(_http);

            // in production enable the ip setting in the service 
            var res = await _ipInfoCheck.getCountryInfoById(ipAddress);

            if (res is not null && res.isSuccess==true ) return Ok(res); 
            else if (res is null && res.isSuccess == true) return NotFound(res);
            return BadRequest(res);

        }


        [HttpPost("check-block")]
        public async Task<IActionResult> CheckIfIpBlocked ()
        {
            string ip = extractIp(_http); 
            // in production enable the ip setting in the service 
            var res = await _ipInfoCheck.checkIpIfBlocked(ip);
            return Ok(res);   
        }

        //[HttpGet("GetAllBlockedAttemp")]
        //public async Task<IActionResult> GetAllBlockedAttemp (int ? pageNo = 1 , int? pageSize = 10 )
        //{
        //    var res = await _ipInfoCheck.GetAllBlockedLoggingAttemps(pageNo, pageSize); 
        //    if (res.isSuccess == true ) return Ok(res);
        //    return BadRequest(res); 
        //}


    }
}
