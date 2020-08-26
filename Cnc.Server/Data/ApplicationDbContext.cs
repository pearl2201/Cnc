using Microsoft.EntityFrameworkCore;

namespace Cnc.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {


        }
    }
}