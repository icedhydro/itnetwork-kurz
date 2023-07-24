using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EvidencePojisteni.Models
{
    public class Insurance
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Druh pojištění je povinný údaj")]
        [Display(Name = "Druh pojištění")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Částka je povinný údaj")]
        [Display(Name = "Částka")]
        [Column(TypeName = "decimal(18,2)")]
		[RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Částka musí být číslo s dvěma desetinnými místy.")]
		public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Předmět pojištění je povinný údaj")]
        [Display(Name = "Předmět pojištění")]
        public string Subject { get; set; }

        [Display(Name = "Platnost od")]
        [Required(ErrorMessage = "Platnost od je povinný údaj")]
        [DataType(DataType.Date)]
        public DateTime? ValidFrom { get; set; }


        [Display(Name = "Platnost do")]
        [Required(ErrorMessage = "Platnost do je povinný údaj")]
        [DataType(DataType.Date)]
        public DateTime? ValidUntil { get; set; }

        [ForeignKey("Insured")]
        public int InsuredId { get; set; }

        public virtual Insured Insured { get; set; }

		public virtual ICollection<InsEvent> InsEvent { get; set; }

		public static List<SelectListItem> GetInsuranceTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Úrazové pojištění", Text = "Úrazové pojištění" },
                new SelectListItem { Value = "Životní pojištění", Text = "Životní pojištění" },
                new SelectListItem { Value = "Cestovní pojištění", Text = "Cestovní pojištění" },
                new SelectListItem { Value = "Pojištění majetku", Text = "Pojištění majetku" },
                new SelectListItem { Value = "Pojištění internetových rizik", Text = "Pojištění internetových rizik" }
            };
        }
    }
}
