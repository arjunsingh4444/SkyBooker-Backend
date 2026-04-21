using SkyBooker.SeatService.Interfaces;

namespace SkyBooker.SeatService.Background;

public class SeatHoldCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SeatHoldCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

            var threshold = DateTime.UtcNow.AddMinutes(-15);

            var expired = await repo.FindExpiredHolds(threshold);

            foreach (var seat in expired)
            {
                seat.Status = "AVAILABLE";
                seat.HeldSince = null;
                await repo.Update(seat);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}