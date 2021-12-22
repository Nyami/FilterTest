using GreenPipes;
using MassTransit;

namespace FilterTest;

/// <summary>
/// Filter to apply a fictitious tenant id to messages
/// </summary>
/// <typeparam name="T"></typeparam>
public class PublishTenantHeaderFilter<T> : IFilter<PublishContext<T>> where T : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("PublishTenantHeaderFilter");
    }

    public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        context.Headers.Set("X-TenantId", Guid.NewGuid().ToString());

        return next.Send(context);
    }
}