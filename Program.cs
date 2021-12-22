using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FilterTest;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((services) =>
            {
                services.AddScoped<TenantContext>();

                services.AddScoped<FakeTenantDbContext>(_ =>
                {
                    var logger = _.GetRequiredService<ILogger<FakeTenantDbContext>>();
                    var ctx = _.GetRequiredService<TenantContext>();
                    return new FakeTenantDbContext(logger, ctx);
                });

                // Would using a lazy dependency perhaps solve the problem?
                services.AddScoped<Lazy<FakeTenantDbContext>>(_ =>
                {
                    var logger = _.GetRequiredService<ILogger<FakeTenantDbContext>>();
                    var ctx = _.GetRequiredService<TenantContext>();
                    return new Lazy<FakeTenantDbContext>(() => new FakeTenantDbContext(logger, ctx));
                });

                services.AddMassTransit(busConfig =>
                {
                    busConfig.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("amqps://guest:guest@localhost:5672");

                        // Add the tenant id to published messages
                        cfg.UsePublishFilter(typeof(PublishTenantHeaderFilter<>), context);

                        // Set the 'tenant context' when consuming a message
                        cfg.UseConsumeFilter(typeof(TenantConsumeContextFilter<>), context);

                        cfg.ConfigureEndpoints(context);
                    });

                    busConfig.AddConsumer<TestConsumer>();
                });

                services.AddSingleton<IHostedService, MassTransitServiceHost>();
            });

        await builder.RunConsoleAsync();
    }
}