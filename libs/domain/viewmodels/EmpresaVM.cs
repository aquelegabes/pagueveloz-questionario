using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PagueVeloz.Domain.ViewModels
{
    public class EmpresaVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required][MaxLength(254)]
        public string UF { get; set; }
        [Required][MaxLength(100)]
        public string NomeFantasia { get; set; }
        [Required][MaxLength(20)]
        public string CNPJ { get; set; }
        public FornecedorVM Fornecedor { get; set; }
    }
}