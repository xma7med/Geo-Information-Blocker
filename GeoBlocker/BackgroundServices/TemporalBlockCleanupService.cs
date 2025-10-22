using GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore;

namespace GeoBlocker.BackgroundServices
{
    public sealed class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IBlockedCountryInMemoryStore _store;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(IBlockedCountryInMemoryStore store) => _store = store;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(_interval);
            try
            {

                while (!stoppingToken.IsCancellationRequested &&
                       await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _store.RemoveExpiredTemporal(DateTimeOffset.UtcNow);
                   
                    Console.WriteLine($"running ...");
                }
                
            }
            catch (OperationCanceledException ex ) 
            {
                Console.WriteLine($"Exception Back ground service {ex.Message}");
            }
        }
    }
}
