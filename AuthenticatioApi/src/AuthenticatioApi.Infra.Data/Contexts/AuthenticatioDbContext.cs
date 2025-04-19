using AuthenticatioApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatioApi.Infra.Data.Contexts
{
    internal class AuthenticatioDbContext(DbContextOptions<AuthenticatioDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
       
        public virtual DbSet<ConfigurationComponent> ConfigurationComponent { get; set; }
        public virtual DbSet<ConfigurationComponentProduct> ConfigurationComponentProduct { get; set; }
        public virtual DbSet<ConfigurationComponentScreen> ConfigurationComponentScreen { get; set; }        
        public virtual DbSet<User> User { get; set; }
    }
}
