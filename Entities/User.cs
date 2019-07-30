using Windows_Backend.Enums;
using Microsoft.AspNetCore.Identity;

namespace Windows_Backend.Entities
{
    public class User: IdentityUser
    {
        public UserType UserType { get; set; }
        public Business Business { get; set; }
    }
}