using System;
using System.Collections.Generic;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Domain.Models
{
    public class Fornecedor : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public bool IsPessoaFisica { get; set; }

        public virtual Empresa Empresa { get; set; }
        public int EmpresaId { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual ICollection<Fone> Fones { get; set; }
    }
}