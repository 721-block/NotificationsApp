namespace EmailNotificator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSingletonSettings<T>(
        this IServiceCollection services,
        IConfigurationManager config,
        string sectionName) where T : class
    {
        var settings = config.GetSection(sectionName).Get<T>();
        services.AddSingleton(settings!);

        return services;
    }
}