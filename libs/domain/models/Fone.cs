using System;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Domain.Models
{
    public class Fone : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Numero { get; set; }

        public int FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
    }
}