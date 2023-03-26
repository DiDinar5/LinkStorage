using System.ComponentModel.DataAnnotations;

namespace LinkStorage.DTO
{
    public class UserViewDTO
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
