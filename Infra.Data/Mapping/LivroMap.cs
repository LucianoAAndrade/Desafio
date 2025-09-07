using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mapping
{
    public class LivroMap : IEntityTypeConfiguration<LivroModel>
    {
        public void Configure(EntityTypeBuilder<LivroModel> builder)
        {
            builder.ToTable("livros");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd(); ;
            builder.Property(x => x.Titulo).HasColumnName("titulo").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Autor).HasColumnName("autor").HasMaxLength(50).IsRequired();
            builder.Property(x => x.QuantidadeDisponivel).HasColumnName("quantidade-disponivel").HasColumnType("smallint").IsRequired();
        }
    }
}
