using Serilog;
using TSFiler.Common.Logging;
using TSFiler.BusinessLogic;

namespace TSFiler.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

        services.AddSingleton<ILoggerFactory>(_ => SerilogFactory.InitLogging());
        services.ConfigureBusinessLogicServices(configuration);
        return services;
    }
}
