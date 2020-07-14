using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.Data.Configurations
{
    public class PessoaConfiguration :
        IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasIndex(p => p.RG).IsUnique();
            builder.Property(p => p.RG).IsRequired().HasMaxLength(20);
            builder.HasIndex(p => p.CPF).IsUnique();
            builder.Property(p => p.CPF).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Nascimento).IsRequired();

            builder.HasOne(o => o.Fornecedor)
                .WithOne(o => o.Pessoa)
                .HasForeignKey<Pessoa>(fk => fk.FornecedorId);
        }
    }
}