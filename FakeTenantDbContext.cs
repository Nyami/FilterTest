using Microsoft.Extensions.Logging;

namespace FilterTest;

public class FakeTenantDbContext
{
    public FakeTenantDbContext(ILogger<FakeTenantDbContext> logger, TenantContext tenantContext)
    {
        logger.LogInformation($"Creating FakeTenantDbContext with {tenantContext.TenantId}");
        TenantId = tenantContext.TenantId;
    }

    public string TenantId { get; }
}