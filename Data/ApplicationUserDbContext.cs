using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using CalendarMate.Models;

namespace CalendarMate.Data;

public class ApplicationUserDbContext : DbContext
{
    public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options)
        : base(options)
    {
    }

    public DbSet<CalendarMate.Models.ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
}
