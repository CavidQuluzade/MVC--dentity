using System.ComponentModel.DataAnnotations;

namespace İdentity.ViewModels.Account
{
    public class AccountRegisterVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password must be entered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Password and Confirm password must be same")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Country must be entered")]
        public string Country {  get; set; }
        [Required(ErrorMessage = "City must be entered")]
        public string City { get; set; }


    }
}
