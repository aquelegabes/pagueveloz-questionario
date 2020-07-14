using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.Data.Configurations
{
    public class FornecedorConfiguration :
        IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100).HasColumnType("TEXT COLLATE NOCASE");

            builder.HasIndex(i => i.CNPJ).IsUnique();
            builder.Property(p => p.CNPJ).HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(p => p.IsPessoaFisica).IsRequired().HasDefaultValue(false);

            builder.HasOne(o => o.Pessoa)
                .WithOne(o => o.Fornecedor)
                .HasForeignKey<Pessoa>(fk => fk.FornecedorId);

            builder.HasMany(m => m.Fones)
                .WithOne(o => o.Fornecedor)
                .HasForeignKey(fk => fk.FornecedorId);
            
            builder.HasOne(o => o.Empresa)
                .WithOne(o => o.Fornecedor)
                .HasForeignKey<Fornecedor>(fk => fk.EmpresaId);
        }
    }
}