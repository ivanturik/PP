using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TSFiler.BusinessLogic.Interfaces;
using TSFiler.BusinessLogic.Services.DataProcessors;
using TSFiler.BusinessLogic.Services.FileProcessors;
using TSFiler.BusinessLogic.Services;

namespace TSFiler.BusinessLogic;

public static class BusinessLogicServicesExtensions
{
    public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileProcessor, PlainTextFileProcessor>();
        services.AddScoped<IFileProcessor, JsonFileProcessor>();
        services.AddScoped<IFileProcessor, XmlFileProcessor>();

        services.AddScoped<IDataProcessor, BasicDataProcessor>();
        services.AddScoped<IDataProcessor, RegexDataProcessor>();

        services.AddScoped<FileService>();
        return services;
    }
}
