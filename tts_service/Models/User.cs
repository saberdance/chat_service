using tts_service.Models.Session;

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
        public string? Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string? Account { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Token { get; set; }
        public int LastSessionId { get; set; } = -1;
        public DateTime LastCall { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public DateTime TokenExpire { get; set; } = DateTime.Now;
        public UserRole Role { get; set; } = UserRole.User;

        public List<ChatSession> ChatSessions = new List<ChatSession>();
    }
}
