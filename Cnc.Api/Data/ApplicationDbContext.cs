using Cnc.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnc.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Payload> Payloads { get; set; }

        public DbSet<ClientInfo> ClientInfos { get; set; }
    }
}