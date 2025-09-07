using Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LivroMap> Livro { get; set; }
    public DbSet<EmprestimoMap> Emprestimo { get; set; }

}
