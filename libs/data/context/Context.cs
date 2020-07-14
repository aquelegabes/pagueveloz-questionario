using Microsoft.EntityFrameworkCore;
using PagueVeloz.Data.Configurations;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.Data.Context
{
    public class PagueVelozContext : DbContext
    {
        public PagueVelozContext(DbContextOptions options) : base(options) { }
        public PagueVelozContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseSqlite(connectionString).Options) { }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Fone> Fones { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optsBuilder)
        {
            optsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder mbuilder)
        {
            mbuilder.ApplyConfiguration(new EmpresaConfiguration());
            mbuilder.ApplyConfiguration(new FoneConfiguration());
            mbuilder.ApplyConfiguration(new FornecedorConfiguration());
            mbuilder.ApplyConfiguration(new PessoaConfiguration());
        }
    }
}