using System;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Domain.Models
{
    public class Pessoa : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime Nascimento { get; set; }

        public int FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public int GetIdade()
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - Nascimento.Year;

            // Go back to the year the person was born in case of a leap year
            if (Nascimento.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}