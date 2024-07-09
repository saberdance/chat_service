using Microsoft.EntityFrameworkCore;
using tts_service.Models;
using tts_service.Models.Chat;
using tts_service.Models.Session;
using tts_service.Models.Stastics;
using tts_service.Models.TtsData;

namespace tts_service.Db
{
    public class ChatContext:DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
   : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatContent> ChatContents { get; set; }
        public DbSet<TtsVoice> TtsVoices { get; set; }
        public DbSet<TtsEngine> TtsEngines { get; set; }
        public DbSet<TtsCall> TtsCalls { get; set; }
        public DbSet<ChatEngine> ChatEngines { get; set; }
    }
}
