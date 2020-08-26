
using Cnc.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnc.Client.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Payload> Payloads { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {


        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite("Data Source=cnc_client.db");
    }
}