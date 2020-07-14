using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.Data.Configurations
{
    public class EmpresaConfiguration :
        IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasIndex(i => i.CNPJ).IsUnique();

            builder.Property(p => p.CNPJ).IsRequired().HasMaxLength(20);
            builder.Property(p => p.NomeFantasia).IsRequired().HasMaxLength(100).HasColumnType("TEXT COLLATE NOCASE");
            builder.Property(p => p.UF).IsRequired().HasMaxLength(254).HasColumnType("TEXT COLLATE NOCASE");
        }
    }
}