using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidencePojisteni.Models
{
    public class RegisterViewModel

    {

        [Required(ErrorMessage = "Jméno je povinný údaj")]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Příjmení je povinný údaj")]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email je povinný údaj")]
        [EmailAddress(ErrorMessage = "Zadejte platný email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon je povinný údaj")]
        [Display(Name = "Telefon")]
        [RegularExpression(@"^\+?\d{3}[\s.-]?\d{3}[\s.-]?\d{3}$", ErrorMessage = "Zadejte platné telefonní číslo.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Ulice a číslo popisné je povinný údaj")]
        [Display(Name = "Ulice a číslo popisné")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Město je povinný údaj")]
        [Display(Name = "Město")]
        public string City { get; set; }

        [Required(ErrorMessage = "PSČ je povinný údaj")]
        [Display(Name = "PSČ")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zadejte platné PSČ")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Zadejte heslo")]
        [StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Zadejte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare("Password", ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; }
    }
}
