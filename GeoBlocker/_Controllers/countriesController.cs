using GeoBlocker._Models.DTOs;
using GeoBlocker._Services.BlockedCountries;
using Microsoft.AspNetCore.Mvc;

namespace GeoBlocker._Controllers
{
    public class countriesController: SharedController
    {
        private readonly IBlockedCountriesService _blockedCountriesService;

        public countriesController( IBlockedCountriesService blockedCountriesService )
        {
            _blockedCountriesService = blockedCountriesService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> addBlockedCountry  (string countryCode  )
        {
            if (countryCode !=null && !countryCodeValidate(countryCode)) return BadRequest("Wrong Country Code check it and must be 2 char ");

            var res = await  _blockedCountriesService.add(countryCode);
            if (!res.isSuccess) return BadRequest(res); 
            if(res.isSuccess && res.data!=null)
            {
                res.data = null;
                 return Ok(res); 
            }
            return Conflict(res);
        }

        [HttpDelete("block/{countryCode}")]

        public async Task<IActionResult> deleteBlockedCountry(string countryCode)
        {
            if (countryCode!=null && !countryCodeValidate(countryCode)) return BadRequest("Wrong Country Code check it and must be 2 char ");

            var res = await _blockedCountriesService.delete(countryCode);
            if (!res.isSuccess) return NotFound();
            return Ok(res); 

        }

        [HttpGet("countries/blocked")]

        public async Task<IActionResult> getallBlockedCountry (string? countryCodeOrName = null, int pageIndex = 1, int PageSize= 10, bool isTempBlocked = false)
        {
            if ( countryCodeOrName !=null &&!countryCodeValidate(countryCodeOrName)) return BadRequest("Wrong Country Code check it and must be 2 char ");
            var res = await _blockedCountriesService.getall(countryCodeOrName ,pageIndex , PageSize ,isTempBlocked ); 
           if (!res.isSuccess)  return BadRequest(res); 
           return Ok(res);  
        }






        [HttpPost("temporal-block")]
        public async Task<IActionResult> addTempBlockedCountry(string countryCode , int timeInMin )
        {

            if (countryCode!=null&&!countryCodeValidate(countryCode) && timeInMin<1&&timeInMin>=1440)return BadRequest("Wrong Country Code check it and must be 2 char ");

            var res =  await  _blockedCountriesService.addTempBlockedCountry(countryCode , timeInMin);    
            if (!res.isSuccess) return BadRequest(res); 
            if(res.isSuccess && res.data!=null)
            {
                res.data = null;// sign for indicate if it added or duplicated 
                return Ok(res); 
            }
            return Conflict(res);
        }


        [HttpGet("GetAllTempBlockedCountry")]
        public async Task<IActionResult> GetAllTempBlockedCountry(string countryCode=null , int pageNo = 1 , int pageSize = 10 )
        {
            if (!string.IsNullOrWhiteSpace(countryCode) || !string.IsNullOrEmpty(countryCode))
            {
                bool check = countryCodeValidate(countryCode);
                if (!check) return BadRequest("Wrong Country Code check it and must be 2 char ");
            }
            var res = await _blockedCountriesService.getall(countryCode, pageNo , pageSize ,true);
            if (!res.isSuccess) return Conflict(res);
            return Ok(res);
        }


    }
}
