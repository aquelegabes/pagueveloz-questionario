using System;
using System.ComponentModel.DataAnnotations;

namespace PagueVeloz.Domain.ViewModels
{
    public class FoneVM
    {
        public int Id { get; set; }
        [Required][MaxLength(30)]
        public string Numero { get; set; }
        public bool New { get; set; }
        public bool Remove { get; set; }
    }
}