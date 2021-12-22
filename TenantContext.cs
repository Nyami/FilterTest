using Microsoft.Extensions.Logging;

namespace FilterTest;

/// <summary>
/// Provides the context of the tenant in this example
/// </summary>
public class TenantContext
{
    public TenantContext(ILogger<TenantContext> logger)
    {
        logger.LogInformation("Creating TenantContext");
    }

    public string TenantId { get; internal set; } = "NotSetYet";
}