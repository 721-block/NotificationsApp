using Microsoft.Extensions.Options;

namespace PushNotificator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSingletonSettings<T>(
        this IServiceCollection services,
        IConfigurationManager config,
        string sectionName) where T : class
    {
        services.Configure<T>(config.GetSection(sectionName));

        services.AddSingleton<T>(x =>
        {
            var a = x.GetService<IOptions<T>>();
            return a!.Value;
        });

        return services;
    }

    public static IServiceCollection AddScopedSettings<T>(
        this IServiceCollection services,
        IConfigurationManager config,
        string sectionName) where T : class
    {
        services.Configure<T>(config.GetSection(sectionName));

        services.AddScoped<T>(x =>
        {
            var a = x.GetService<IOptionsSnapshot<T>>();
            return a!.Value;
        });

        return services;
    }
}