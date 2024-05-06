using System.Reflection;
using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

    /// <summary>
    /// Adds Swagger generation services with versioning support to the specified IServiceCollection.
    /// </summary>
    /// <param name="serviceCollection">The IServiceCollection to add services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            IApiVersionDescriptionProvider provider = serviceCollection.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (ApiVersionDescription apiDescription in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(apiDescription.GroupName, new OpenApiInfo
                {
                    Title = "Identity API",
                    Version = apiDescription.ApiVersion.ToString(),
                    Description = "Identity API ensures secure user authentication, facilitates user registration, and offers features for managing user accounts effectively. Developers can integrate this API into their applications to handle user-related tasks seamlessly."
                });
            }

            //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Configure JWT Bearer authentication for Swagger
            OpenApiSecurityScheme securityScheme = new()
            {
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    new List<string>()
                }
            });
        });

        return serviceCollection;
    }

    /// <summary>
    /// Adds API versioning services to the specified IServiceCollection.
    /// </summary>
    /// <param name="serviceCollection">The IServiceCollection to add services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return serviceCollection;
    }

    /// <summary>
    /// Adds authentication services with JWT Bearer support to the specified IServiceCollection.
    /// </summary>
    /// <param name="serviceCollection">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration manager to retrieve JWT settings from.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAuthenticationConfiguration(
        this IServiceCollection serviceCollection,
        ConfigurationManager configuration)
    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
            };
        });

        return serviceCollection;
    }
}

