namespace tts_service.Models
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        public int Id { get; set; }
        public string? UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? Account { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Token { get; set; }
        public DateTime LastCall { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public DateTime TokenExpire { get; set; } = DateTime.Now;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
