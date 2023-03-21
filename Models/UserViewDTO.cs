using System.ComponentModel.DataAnnotations;

namespace LinkStorage.Models
{
    public class UserViewDTO
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public Authorization Email { get; set; }
    }
}
