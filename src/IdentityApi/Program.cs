using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XkliburSolutions.IdentityApi.Domain.Entities;
using XkliburSolutions.IdentityApi.Extensions;
using XkliburSolutions.IdentityApi.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<IdentityApiDbContext>(
    options => options.UseSqlite(configuration.GetConnectionString("IdentityDb")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityApiDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthenticationConfiguration(configuration);
builder.Services.AddAuthorization();
builder.Services.AddLocalization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityApiDbContext>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Identity Api v1.0");
    });

    using (IServiceScope scope = app.Services.CreateScope())
    {
        IdentityApiDbContext db = scope.ServiceProvider.GetRequiredService<IdentityApiDbContext>();
        db.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.ConfigureMapping();

app.Run();
