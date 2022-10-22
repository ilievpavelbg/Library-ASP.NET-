using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; } = new List<ApplicationUserBook>();
    }
}
