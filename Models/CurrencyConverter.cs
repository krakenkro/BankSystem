using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystemBota.Models
{
    [Keyless]
    public class CurrencyConverter
    {
        [Display(Name = "Тенге в")]
        public int Tenge { get; set; }

        [Display(Name = "Result")]
        public string Result { get; set; }

        [Display(Name = "Currency")]
        public CurrencyType CurrencyType { get; set; }
    }
    public enum CurrencyType
    {
        Rubl,
        Dollar,
        Uan
    }
}
