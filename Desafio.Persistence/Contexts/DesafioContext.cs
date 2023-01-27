using Microsoft.EntityFrameworkCore;

namespace Desafio.Persistence.Contexts
{
    public class DesafioContext : DbContext
    {
        public DesafioContext(DbContextOptions<DesafioContext> options) : 
            base(options) => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        public DbSet<Domain.Cliente> Cliente { get; set; }
    }
}