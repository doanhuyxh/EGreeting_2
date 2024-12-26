using Microsoft.AspNetCore.Identity;

namespace EGreeting.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public decimal? TotalMoney { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
