using LinkStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace LinkStorage.DTO
{
    public class LoginResponceDTO
    {
        [Required]
        public User User { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
