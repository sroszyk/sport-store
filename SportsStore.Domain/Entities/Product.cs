using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę produktu.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę podać opis.")]
        [DataType(DataType.MultilineText), Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Cena")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proszę podać dodatnią cenę.")]
        public decimal Price { get; set; }
        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "Proszę określić kategorię.")]
        public string Category { get; set; }
    }
}
