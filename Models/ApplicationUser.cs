using Microsoft.AspNetCore.Identity;

namespace JWT_Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime Addedon { get; set; }
    }
}
