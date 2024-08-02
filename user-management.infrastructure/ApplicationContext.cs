using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using user_management.domain.Entities;

namespace user_management.infrastructure
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
