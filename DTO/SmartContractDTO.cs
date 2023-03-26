using Microsoft.Build.Framework;

namespace LinkStorage.DTO
{
    public class SmartContractDTO
    {
        public uint Id { get; set; }
        [Required]
        public string LinkToContract { get; set; }
        [Required]
        public DateTime DateTimeCreated { get; set; }
    }
}
