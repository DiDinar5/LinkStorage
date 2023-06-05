using System.ComponentModel.DataAnnotations;

namespace LinkStorage.DTO
{
    public class UserDTO
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
