namespace LinkStorage.Models
{
    public class User
    {
        public uint Id { get; set; }   
        public string Name { get; set; }
        public string Surname { get; set; }
        public Authorization Authorization { get; set; }
        public uint AuthorizationId { get; set; }
        public List<SmartContract> SmartContracts { get; set; }
    }
}
