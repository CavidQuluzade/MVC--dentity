using System.ComponentModel.DataAnnotations;

namespace İdentity.Areas.Admin.Models.Message
{
    public class MessageSendVM
    {
        [Required(ErrorMessage = "Tittle required")]
        [MinLength(3, ErrorMessage = "Tittle must contain at least 3 character")]
        [Display(Name = "Tittle")]
        public string Tittle { get; set; }
        [Required(ErrorMessage = "Description required")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
