using user_management.infrastructure;

namespace user_management.api.Seeder
{
    public class DataSeeder
    {
        public static async Task SeedData(ApplicationContext dbContext)
        {
            
            await dbContext.SaveChangesAsync();
        }
    }
}
