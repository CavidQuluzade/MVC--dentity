using Microsoft.AspNetCore.Identity;

namespace İdentity.Data.Entities
{
    public class User : IdentityUser
    {
        public string City { get; set; }
        public string Country { get; set; }
        public bool isSubscribed { get; set; }
    }
}
