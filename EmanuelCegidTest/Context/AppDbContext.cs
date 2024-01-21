using EmanuelCegidTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmanuelCegidTest.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet <Customers> Customers { get; set; }
        public DbSet <SalesItems> SalesItems { get; set; }
    }
}
