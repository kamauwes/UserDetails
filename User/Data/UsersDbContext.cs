using Microsoft.EntityFrameworkCore;
using RegionsUser.Models.Domains;
using User.Models.Domains;

namespace User.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<ClientPrivacy> UserPrivacy { get; set; }
    }
    
}
