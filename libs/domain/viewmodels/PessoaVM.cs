using System;
using System.ComponentModel.DataAnnotations;

namespace PagueVeloz.Domain.ViewModels
{
    public class PessoaVM
    {
        public int Id { get; set; }
        [Required][MaxLength(20)]
        public string CPF { get; set; }
        [Required][MaxLength(20)]
        public string RG { get; set; }
        [Required][DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }
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