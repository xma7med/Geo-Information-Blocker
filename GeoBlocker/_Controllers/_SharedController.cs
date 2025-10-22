using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace GeoBlocker._Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SharedController: ControllerBase
    {
        [NonAction]
        public bool IsValidIp(string? ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return IPAddress.TryParse(ipAddress, out _);
        }
        [NonAction]
        public string extractIp (IHttpContextAccessor http)
        {
            ///var  ip =  http.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            ///var ip = http.HttpContext?.Connection.RemoteIpAddress ;
           ///var ip3 =  HttpContext.GetServerVariable("REMOTE_ADDR"); 
            string ip = http.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if(ip == null) 
                ip = HttpContext.GetServerVariable("REMOTE_ADDR");

            //IPAddress remoteIp = null;   
            //string ip =http.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if(ip == null) 
                ip = http.HttpContext?.Connection.RemoteIpAddress.ToString();
            //ip = http.HttpContext?.Connection.RemoteIpAddress.AddressFamily.ToString();
            if (string.IsNullOrWhiteSpace(ip) || ip == "::1" || ip == "127.0.0.1")
                ip = "154.183.151.251"; // test IP for local runs

            /// //else
            /// //{
            /// //    // Header may contain multiple IPs separated by comma:
            /// //    ip = ip.Split(',')[0].Trim();
            /// //}
            if (IPAddress.TryParse(ip, out var ipAddress))
                return ipAddress.ToString();
            
            ///IPAddress.IsLoopback(IPAddress.Loopback);
            ///iipAddresspAddress.ToString();
            ///if (ipAddress.IsIPv6LinkLocal)
            ///{
            ///    ip= "127.0.0.1";
            ///}

           ///ip = ipAddress.ToString();
           ///if (IPAddress. IsLoopback(remoteIp) && remoteIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
           ///{
           ///    ipAddress= System.Net.IPAddress.Loopback; // "127.0.0.1" :contentReference[oaicite:0]{index=0}
           ///    IPAddress.TryParse(ipAddress.ToString(), out var ipAdd);
           ///    return ipAdd.ToString();    
           ///}

            return null!;


        }


        [NonAction]

        public bool countryCodeValidate(string ? countCode )
        { 
            if (countCode.Length != 2)
            {
                return false;
            }
            if (countCode[0] == countCode[1])
                return false;
            return true;
        }
    }
}
