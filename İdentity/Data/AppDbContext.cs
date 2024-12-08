using İdentity.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace İdentity.Data
{

    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
