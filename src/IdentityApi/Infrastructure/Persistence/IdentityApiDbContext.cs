using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XkliburSolutions.IdentityApi.Domain.Constants;
using XkliburSolutions.IdentityApi.Domain.Entities;

namespace XkliburSolutions.IdentityApi.Infrastructure.Persistence;

/// <summary>
/// Represents the database context used by the Identity API for managing user data and identity information.
/// This class provides the necessary methods and properties to interact with the underlying database
/// and perform operations such as CRUD (Create, Read, Update, Delete) on user accounts, roles, and claims.
/// </summary>
/// <remarks>
/// Initializes a new instance of the IdentityApiDbContext class using the specified options.
/// The options include configurations such as the database provider, connection string, and other settings
/// that are necessary for the context to interact with the database.
/// </remarks>
public class IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    /// <summary>
    /// Configures the schema needed for the identity system by overriding the OnModelCreating method.
    /// This method is called when the model for a derived context has been initialized, but
    /// before the model has been locked down and used to initialize the context. The default
    /// implementation of this method does nothing, but it can be overridden in a derived class
    /// such that the model can be further configured before it is locked down.
    /// </summary>
    /// <param name="builder">Provides a simple API for configuring a model that defines the shape of your entities, 
    /// the relationships between them, and how they map to the database.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        #region Roles seeding
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole {
                Id = ApplicationRoles.Administrator.ToLower(),
                Name = ApplicationRoles.Administrator,
                NormalizedName = ApplicationRoles.Administrator.ToUpper()
            },
            new IdentityRole {
                Id = ApplicationRoles.User.ToLower(),
                Name = ApplicationRoles.User,
                NormalizedName = ApplicationRoles.User.ToUpper()
            }
        );
        #endregion
    }
}
