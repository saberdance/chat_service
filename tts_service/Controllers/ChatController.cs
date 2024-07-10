using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using tts_service.Db;
using tts_service.Models.Protocol;
using tts_service.Models.Session;

namespace tts_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatContext _context;
        public ChatController(ChatContext chatContext)
        {
            _context = chatContext;
        }

        [HttpGet("get_user_sessions")]
        public async Task<ActionResult<BaseResponse<List<ChatSession>>>> GetUserSessions(string user_id)
        {
            var sessions = await _context.ChatSessions.Where(s => s.UserGuid == user_id).ToListAsync();
            return new SuccessResponse<List<ChatSession>>(sessions);
        }

        [HttpGet("get_user_session_info")]
        public async Task<ActionResult<BaseResponse<ChatSession>>> GetUserSessionInfo(string user_id,int session_id)
        {
            var user = await _context.Users.Where(o=>o.Guid == user_id).FirstOrDefaultAsync();
            if (user == default)
            {
                return NotFound(new InvalidCredentialResponse());
            }
            var session = await _context.ChatSessions.Include(o=>o.ChatContents).Where(o=>o.Id == session_id).FirstOrDefaultAsync();
            if (session == null || session.UserId != user.Id)
            {
                return NotFound();
            }
            return Ok(new SuccessResponse<ChatSession>(session));
        }

        [HttpGet("get_user_chat_content")]
        public async Task<ActionResult<BaseResponse<ChatContent>>> GetUserChatContent(string user_id, int session_id, int content_id)
        {
            var content = await _context.ChatContents.Where(c => c.SessionId == session_id && c.Id == content_id && c.UserGuid == user_id).FirstOrDefaultAsync();
            if (content == null)
            {
                return NotFound();
            }
            return Ok(new SuccessResponse<ChatContent>(content));
        }

        [HttpPost("create_user_session")]
        public async Task<ActionResult<BaseResponse<ChatSession>>> CreateChatSession(CreateChatSessionRequest request)
        {
            var user  = await _context.Users.Where(o=>o.Guid == request.UserId).FirstOrDefaultAsync();
            if (user == default)
            {
                return NotFound(new UserNotFoundResponse("User not found"));
            }
            var engine = await _context.ChatEngines.FirstOrDefaultAsync(e => e.Name == request.EngineName);
            if (engine == default)
            {
                return NotFound(new InvalidParamResponse("Engine not found"));
            }
            var voice = await _context.TtsVoices.FirstOrDefaultAsync(v => v.Name == request.VoiceName);
            if (voice  == default)
            {
                return NotFound(new InvalidParamResponse("Voice not found"));
            }
            var ttsEngine = await _context.TtsEngines.Where(o=>o.Name == voice.Engine).FirstOrDefaultAsync();
            var session = new ChatSession
            {
                UserGuid = request.UserId,
                Title = request.Title,
                ChatEngineId = engine.Id,
                TtsEngineId = engine.Id,
                TtsVoiceId = ttsEngine.Id,
                UserId = user.Id
            };
            await _context.ChatSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            _context.Entry(session);
            return Ok(new SuccessResponse<ChatSession>(session));
        }
    }
}
