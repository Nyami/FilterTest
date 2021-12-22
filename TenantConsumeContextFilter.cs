using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FilterTest;

public class TenantConsumeContextFilter<T> :
    IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly TenantContext _tenantContext;
    private readonly ILogger<TenantConsumeContextFilter<T>> _logger;

    public TenantConsumeContextFilter(TenantContext tenantContext, ILogger<TenantConsumeContextFilter<T>> logger)
    {
        _tenantContext = tenantContext;
        _logger = logger;
        _logger.LogInformation("Creating TenantContextFilter");
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("TenantConsumeContextFilter");
    }

    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var tenantId = context.Headers.Get("X-TenantId", string.Empty);
        _tenantContext.TenantId = tenantId;
        _logger.LogInformation($"Reading header for tenant, tenantId is {tenantId}");
        return next.Send(context);
    }
}