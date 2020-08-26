using Cnc.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnc.Insfrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Payload> Payloads { get; set; }

        public DbSet<ClientInfo> ClientInfos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
    }
}