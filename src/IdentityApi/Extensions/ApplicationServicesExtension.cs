using System.Runtime.CompilerServices;
using XkliburSolutions.Familia.Handlers;

namespace XkliburSolutions.Familia.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ApplicationServicesExtension
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to configure.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        // Register scoped services
        serviceCollection.AddScoped<AuthenticationHandler>();
        serviceCollection.AddScoped<RegistrationHandler>();
        serviceCollection.AddScoped<UsersHandler>();

        return serviceCollection;
    }
}

