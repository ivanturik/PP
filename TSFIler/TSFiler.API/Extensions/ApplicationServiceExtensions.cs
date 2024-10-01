using TSFiler.BusinessLogic;

namespace TSFiler.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBusinessLogicServices(configuration);
        return services;
    }
}
