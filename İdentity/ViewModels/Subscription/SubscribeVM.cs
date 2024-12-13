using System.ComponentModel.DataAnnotations;

namespace İdentity.ViewModels.Subscription
{
    public class SubscribeVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

    }
}
