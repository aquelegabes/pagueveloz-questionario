using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PagueVeloz.Domain.ViewModels
{
    public class FornecedorVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required][MaxLength(100)]
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public bool IsPessoaFisica { get; set; }
        public int EmpresaId { get; set; }
        public IEnumerable<FoneVM> Fones { get;set; }
        public PessoaVM Pessoa { get; set; }
        public bool New { get; set; }
        public bool Remove { get; set; }
    }
}