using System.ComponentModel.DataAnnotations;

namespace İdentity.ViewModels.Account
{
    public class AccountResetPasswordVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password must be entered")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm your password")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and Confirm password must be same")]
        public string ConfirmNewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
