using GeoBlocker._Models.DTOs;
using GeoBlocker._Models.Entities;
using System.Collections.Concurrent;

namespace GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore
{
    public class BlockedCountriesInMemoryStore: IBlockedCountryInMemoryStore
    {
        // for blocked country 
        private readonly ConcurrentDictionary<string, BlockedCountry> _blocked = new ConcurrentDictionary<string, BlockedCountry>(StringComparer.OrdinalIgnoreCase);
        //private readonly ConcurrentDictionary<BlockedCountry ,string > _blocked  = new ConcurrentDictionary<BlockedCountry ,string >();   
        private readonly ConcurrentBag<LoggingDto> _logging  = new ConcurrentBag<LoggingDto>();

        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedTemp = new ConcurrentDictionary<string, BlockedCountry>(StringComparer.OrdinalIgnoreCase);

        public async Task<bool> addBlocked(string countryCode, bool? isTempBlock = false, DateTimeOffset? ExpTime = null)
        {
            //try
            //{
            if(_blockedTemp.ContainsKey(countryCode)   )
                return false;
            if(_blocked.ContainsKey(countryCode))
                return false;   
            countryCode = countryCode.ToUpperInvariant();

            if (isTempBlock==true && ExpTime != null)
            {
                var obj = new BlockedCountry()
                {
                    Code = countryCode,
                    IsTemporal = isTempBlock,
                    ExpiresAfter = ExpTime.Value
                };

               bool tRes =  _blockedTemp.TryAdd(countryCode, obj);
                return tRes;
            }

            bool res = _blocked.TryAdd(countryCode, new BlockedCountry(countryCode));
            return res;
            //}
            //catch (Exception ex )
            //{

            //    Console.WriteLine( ex.Message) ;
            //    return false;   
            //}
        }

        public async Task<bool> deleteBlocked(string countryCode)
        {
            //try
            //{
            //string res; 
            countryCode= countryCode.ToUpperInvariant();


            bool isDeleted = _blocked.TryRemove(countryCode, out _);
            if (!isDeleted)  isDeleted= _blockedTemp.TryRemove(countryCode, out _);
            return isDeleted;

            //if (isDeleted)
            //    return res = "Deleted"; 
            //return ""


            //}
            //catch (Exception ex )
            //{

            //    Console.WriteLine(ex.Message);
            //    return false;   
            //}
        }


        // Filteration and Pagenation 
        public async Task< List<BlockedCountry>> getAllBlockedCountry(string? countryCodeOrName = null, bool isTempBlocked = false/*, int page = 1, int size = 10*/)
        {
            string search = countryCodeOrName?.ToUpperInvariant() ?? "";
            //if (page < 1) page = 1;
            //if (size < 1 || size > 100) size = 10;

            IEnumerable<KeyValuePair<string, BlockedCountry>> query = _blocked;
            if (isTempBlocked == true)
                query = _blockedTemp;
            else
                query = query.Concat(_blockedTemp);
            if (!string.IsNullOrEmpty(countryCodeOrName))
            {
                // filter by name or code 
                query = query.Where(entity => (!string.IsNullOrEmpty( entity.Value?.Code)) && entity.Value.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
                    ||(!string.IsNullOrEmpty(entity.Value.Name))&&entity.Value.Name.Contains(search,StringComparison.OrdinalIgnoreCase)
                    || (!string.IsNullOrEmpty(entity.Key))&&entity.Key.Contains(search, StringComparison.OrdinalIgnoreCase));

            }

            int count = query.Count();


            var res = query.Select(ent => ent.Value).Where(val=>val!=null).Distinct().ToList();

            return res;

        }



        public async Task addLoggedStamp( LoggingDto data )
        {
            _logging.Add(data);
        }


        public async Task<ConcurrentBag<LoggingDto>> GetallLoggingAttemps ()
        {
            return _logging;    
        }



        public int RemoveExpiredTemporal(DateTimeOffset nowUtc)
        {
            var removed = 0;
            foreach (var kv in _blockedTemp)
            {
                if (kv.Value.ExpiresAfter <= nowUtc)
                {
                    if (_blockedTemp.TryRemove(kv.Key, out _)) removed++;
                }
            }
            return removed;
        }


    }
}
