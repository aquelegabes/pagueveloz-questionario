using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.Data.Configurations
{
    public class FoneConfiguration :
        IEntityTypeConfiguration<Fone>
    {
        public void Configure(EntityTypeBuilder<Fone> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasIndex(i => i.Numero);
            builder.Property(p => p.Numero).IsRequired().HasMaxLength(30);

            builder.HasOne(o => o.Fornecedor)
                .WithMany(m => m.Fones)
                .HasForeignKey(fk => fk.FornecedorId);
        }
    }
}