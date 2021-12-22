using MassTransit;
using Microsoft.Extensions.Logging;

namespace FilterTest;

public class TestConsumer : IConsumer<MyEvent>
{
    private readonly Lazy<FakeTenantDbContext> _lazyTenantDbContext;
    private readonly FakeTenantDbContext _tenantDbContext;
    private readonly string _tenantId;
    private readonly ILogger<TestConsumer> _logger;

    public TestConsumer(
        Lazy<FakeTenantDbContext> lazyTenantDbContext,
        FakeTenantDbContext tenantDbContext,
        TenantContext tenantContext,
        ILogger<TestConsumer> logger)
    {
        _lazyTenantDbContext = lazyTenantDbContext;
        _tenantDbContext = tenantDbContext;
        _tenantId = tenantContext.TenantId;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MyEvent> context)
    {
        _logger.LogInformation($"Tenant from ctor was {_tenantId}");
        _logger.LogInformation($"Tenant from DbContext is {_tenantDbContext.TenantId}");
        _logger.LogInformation($"Tenant from Lazy DbContext is {_lazyTenantDbContext.Value.TenantId}");
        return Task.CompletedTask;
    }
}