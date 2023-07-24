using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je povinný údaj")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinný údaj")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [Display(Name = "Pamatuj si mě")]
        public bool RememberMe { get; set; }
    }
}
