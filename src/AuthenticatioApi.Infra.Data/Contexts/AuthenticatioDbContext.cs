using AuthenticatioApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatioApi.Infra.Data.Contexts
{
    internal class AuthenticatioDbContext(DbContextOptions<AuthenticatioDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
       
        public virtual DbSet<MenuComponent> MenuComponent { get; set; }
        public virtual DbSet<MenuProduct> MenuProduct { get; set; }
        public virtual DbSet<MenuScreen> MenuScreen { get; set; }        
        public virtual DbSet<User> User { get; set; }
    }
}
