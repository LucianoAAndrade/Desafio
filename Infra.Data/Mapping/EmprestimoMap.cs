using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mapping
{
    public class EmprestimoMap : IEntityTypeConfiguration<EmprestimoModel>
    {
        public void Configure(EntityTypeBuilder<EmprestimoModel> builder)
        {
            builder.ToTable("emprestimos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd(); ;
            builder.Property(x => x.DataEmprestimo).HasColumnName("data-emprestimo").IsRequired();
            builder.Property(x => x.DataDevolucao).HasColumnName("data-devolucao").IsRequired(false);
            builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(1).HasColumnType("smallint").IsRequired();

            builder.Property(d => d.LivroId).HasColumnName("livro-id").HasColumnType("uniqueidentifier");
            builder.HasOne(x => x.Livro).WithMany().HasForeignKey(x => x.LivroId);
        }
    }
}
