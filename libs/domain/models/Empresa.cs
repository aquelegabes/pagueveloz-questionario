using System;
using System.Collections.Generic;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Domain.Models
{
    public class Empresa : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UF { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }
    }
}