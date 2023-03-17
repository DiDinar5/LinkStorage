namespace LinkStorage.Models
{
    public class SmartContract
    {
        public uint Id { get; set; }
        public string LinkToContract { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
