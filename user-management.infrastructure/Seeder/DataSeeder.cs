using Microsoft.AspNetCore.Identity;
using user_management.domain.Entities;
using EnumGender = user_management.domain.Enums;

namespace user_management.infrastructure.Seeder
{
    public class DataSeeder
    {
        public static async Task SeedData(ApplicationContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!dbContext.Roles.Any())
            {
                await AddRoles(dbContext, userManager, roleManager);
            }

            AppUser appUser  = null;
            if (!dbContext.Users.Any())
            {
                appUser = await AddUser(userManager);
            }

            if (!dbContext.Tenants.Any() && appUser != null)
            {
                await AddTenant(appUser, dbContext);
            }

            if (!dbContext.Genders.Any())
            {
                await AddGenders(dbContext);
            }
        }

        public static async Task AddRoles(ApplicationContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Standard Roles
            List<string> roles = new List<string>
            {
                "Admin", //Clay Solutions admin
                "SuperAdmin", //Clay Solutions super admin
                "TenantAdmin",
                "TenantSuperAdmin",
                "StaffMember", //Tenant's staff
                "Employee" //Clay Solutions staff
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }

        public static async Task<AppUser> AddUser(UserManager<AppUser> userManager)
        {
            var address = new Address()
            {
                StreetNo = "205",
                City = "Ikeja",
                State = "Lagos",
                Country = "Nigeria"
            };
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Chukwuebuka",
                MiddleName = "Joseph",
                LastName = "Enyelu",
                UserName = "enyelu",
                DateOfBirth = DateTime.UtcNow,
                Email = "enyelu.joseph@outlook.com",
                PhoneNumber = "+2347062926449",
                Gender = EnumGender.Gender.Male.ToString(),
                IsActive = true,
                IsTenantStaff = false,
                Avatar = "http://placehold.it/32x32",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Address = address
            };
            await userManager.CreateAsync(user, "Password@123");
            await userManager.AddToRoleAsync(user, "SuperAdmin");

            var addressuser2 = new Address()
            {
                StreetNo = "205",
                City = "Ikeja",
                State = "Lagos",
                Country = "Nigeria"
            };
            var user2 = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Noord",
                MiddleName = "Ilona",
                LastName = "Ihor",
                UserName = "ihor",
                DateOfBirth = DateTime.UtcNow,
                Email = "i.aleshchenko@saltosystems.com",
                PhoneNumber = "+2347062926449",
                Gender = EnumGender.Gender.Male.ToString(),
                IsActive = true,
                IsTenantStaff = true,
                Avatar = "http://placehold.it/32x32",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Address = address
            };

            await userManager.CreateAsync(user2, "Password@123");
            await userManager.AddToRoleAsync(user2, "TenantSuperAdmin");

            return user2;
        }

        public static async Task AddTenant(AppUser appuser, ApplicationContext dbContext)
        {
            var Tenant = new Tenant
            {
                Name = "ShellBP",
                LockIds = new List<string> { "JEer5oU8zS3oA8Iwxu6aLrMoW8Ua3sq5GQKDhcNqow8HYoeIXNptHIQAYkfPlHiJ" },
                StaffMembers = new List<Staff>
                {
                    new Staff{AppUserId = Guid.Parse(appuser.Id)}
                }
            };
            await dbContext.Tenants.AddAsync(Tenant);
            await dbContext.SaveChangesAsync();
        }

        public static async Task AddGenders(ApplicationContext dbContext)
        {
            var genders = new List<Gender>
            {
                new Gender{Name = EnumGender.Gender.Male.ToString()},
                new Gender{Name = EnumGender.Gender.Female.ToString()},
                new Gender{Name = EnumGender.Gender.Other.ToString()}
            };

            await dbContext.Genders.AddRangeAsync(genders);
            await dbContext.SaveChangesAsync();
        }
    }
}
