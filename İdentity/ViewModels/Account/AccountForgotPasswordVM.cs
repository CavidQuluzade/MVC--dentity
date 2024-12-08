using System.ComponentModel.DataAnnotations;

namespace İdentity.ViewModels.Account
{
    public class AccountForgotPasswordVM
    {
        [Required(ErrorMessage ="Error is required")]
        public string Email { get; set; }
    }
}
