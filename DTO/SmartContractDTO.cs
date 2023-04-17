using Microsoft.Build.Framework;

namespace LinkStorage.DTO
{
    public class SmartContractDTO
    {
        [Required]
        public string LinkToContract { get; set; }
    }
}
