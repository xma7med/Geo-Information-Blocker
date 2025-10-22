using GeoBlocker._Services.IPCheckUp;
using Microsoft.AspNetCore.Mvc;

namespace GeoBlocker._Controllers
{
    public class logsController:SharedController
    {
        private readonly IIpInfoCheckUpService _ipInfoCheck;

        public logsController(IIpInfoCheckUpService ipInfoCheck)
        {
            _ipInfoCheck = ipInfoCheck;
        }


        [HttpGet("blocked-attempts")]
        public async Task<IActionResult> GetAllBlockedAttemp(int? pageNo = 1, int? pageSize = 10)
        {
            var res = await _ipInfoCheck.GetAllBlockedLoggingAttemps(pageNo, pageSize);
            if (res.isSuccess == true) return Ok(res);
            return BadRequest(res);
        }


    }
}
