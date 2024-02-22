using Microsoft.AspNetCore.Identity;

namespace AcerProProject1.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
