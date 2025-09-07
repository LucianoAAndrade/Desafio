using Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infra.Data.Config
{
    public class Context : DbContext
    {
        private IConfiguration _config;

        public Context(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LivroMap());
            modelBuilder.ApplyConfiguration(new EmprestimoMap());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionslBuilder)
        {
            optionslBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }
    }
}
