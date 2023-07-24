using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EvidencePojisteni.Models
{
    public class InsEvent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Název události je povinný údaj.")]
        [Display(Name = "Název události")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Datum události je povinný údaj.")]
        [Display(Name = "Datum události")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Popis události je povinný údaj.")]
        [Display(Name = "Popis události")]
        public string Description { get; set; }

        [Display(Name = "Výše plnění")]
        [Column(TypeName = "decimal(18,2)")]
		[RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Částka musí být číslo s dvěma desetinnými místy.")]
		public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Stav je povinný údaj")]
        [Display(Name = "Stav události")]
		public string Status { get; set; }

		[ForeignKey("Insurance")]
        public int InsuranceId { get; set; }

        public virtual Insurance Insurance { get; set; }

		public static List<SelectListItem> GetStatusTypes()
		{
			return new List<SelectListItem>
			{
				new SelectListItem { Value = "Nové oznámení", Text = "Nové oznámení", Selected = true },
				new SelectListItem { Value = "Dokumentace", Text = "Dokumentace" },
				new SelectListItem { Value = "Posouzení", Text = "Posouzení" },
				new SelectListItem { Value = "Schváleno", Text = "Schváleno" },
				new SelectListItem { Value = "Zamítnuto", Text = "Zamítnuto" },
				new SelectListItem { Value = "Vyplaceno", Text = "Vyplaceno" },
                new SelectListItem { Value = "Uzavřeno", Text = "Uzavřeno" }
			};
		}
	}
}
