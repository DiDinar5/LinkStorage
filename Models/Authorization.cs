namespace LinkStorage.Models
{
    public class Authorization
    {
        public uint UserId { get; set; }
        public virtual User UserInfo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
