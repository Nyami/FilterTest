using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FilterTest;

public class MassTransitServiceHost : IHostedService, IDisposable
{
    private readonly IBusControl _busControl;
    private readonly ILogger<MassTransitServiceHost> _logger;
    private Timer _timer = null!;

    public MassTransitServiceHost(IBusControl busControl, ILogger<MassTransitServiceHost> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting MassTransit Service Host");
        await _busControl.StartAsync(cancellationToken);

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        _logger.LogInformation("MassTransit Service Host Started");
    }

    private void DoWork(object? state)
    {
        _busControl.Publish<MyEvent>(new { });
        _busControl.Publish<MyEvent>(new { });
        _busControl.Publish<MyEvent>(new { });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping MassTransit Service Host");
        await _busControl.StopAsync(cancellationToken);
        _timer.Change(Timeout.Infinite, 0);
        _logger.LogInformation("MassTransit Service Host Stopped");
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}