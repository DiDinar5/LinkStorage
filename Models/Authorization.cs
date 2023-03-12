namespace LinkStorage.Models
{
    public class Authorization
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User UserInfo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
