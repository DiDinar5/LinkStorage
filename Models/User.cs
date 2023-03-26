namespace LinkStorage.Models
{
    public class User
    {
        public uint Id { get; set; }   
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<SmartContract> SmartContracts { get; set; }
    }
}
