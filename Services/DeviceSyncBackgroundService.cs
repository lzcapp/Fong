namespace Fong.Services {
    public class DeviceSyncBackgroundService : BackgroundService {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DeviceSyncBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // run every 5 min

        public DeviceSyncBackgroundService(
            IServiceProvider serviceProvider, ILogger<DeviceSyncBackgroundService> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            _logger.LogInformation("Background device sync service started.");

            while (!stoppingToken.IsCancellationRequested) {
                using (var scope = _serviceProvider.CreateScope()) {
                    var syncService = scope.ServiceProvider.GetRequiredService<FingService>();
                    await syncService.SyncDevicesAsync();
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}