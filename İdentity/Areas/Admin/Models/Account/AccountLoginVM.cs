using System.ComponentModel.DataAnnotations;

namespace İdentity.Areas.Admin.Models.Account
{
    public class AccountLoginVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password must be entered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? returnURL { get; set; }
    }
}
