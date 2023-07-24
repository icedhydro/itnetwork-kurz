using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models
{
	public class ChangePasswordViewModel
	{

		[Required(ErrorMessage = "Aktuální heslo je povinný údaj")]          
		[DataType(DataType.Password)]                                       
		[Display(Name = "Aktuální heslo")]            
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "Nové heslo je povinný údaj")]
		[StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2} a nejvíc {1} znaků.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Nové heslo")]
		public string NewPassword { get; set; }

		[Compare("NewPassword", ErrorMessage = "Zadaná hesla se neshodují")]
		[DataType(DataType.Password)]
		[Display(Name = "Potvrzení nového hesla")]
		public string ConfirmPassword { get; set; }

	}

}
