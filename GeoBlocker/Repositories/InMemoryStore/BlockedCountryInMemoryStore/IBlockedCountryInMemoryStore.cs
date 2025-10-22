using GeoBlocker._Models.DTOs;
using GeoBlocker._Models.Entities;
using System.Collections.Concurrent;

namespace GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore
{
    public interface IBlockedCountryInMemoryStore
    {
        Task<bool> addBlocked(string countryCode , bool? isTempBlock = false, DateTimeOffset? ExpTime = null);
        Task<bool> deleteBlocked(string countryCode);
        Task<List<BlockedCountry>> getAllBlockedCountry(string? countryCodeOrName, bool isTempBlocked = false/*, int page = 1, int size = 10*/);


        public  Task addLoggedStamp(LoggingDto data);
        public  Task<ConcurrentBag<LoggingDto>> GetallLoggingAttemps();

        public int RemoveExpiredTemporal(DateTimeOffset nowUtc);

    }
}
