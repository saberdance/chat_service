using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tts_service.Db;
using tts_service.Models.Protocol;
using tts_service.Models.TtsData;
using DoubaoTtsSdk;
using tts_service.Services;
using tsubasa;
using tts_service.Models.Stastics;
using Microsoft.AspNetCore.Authorization;

namespace tts_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TtsController : ControllerBase
    {
        private readonly ChatContext _context;
        public TtsController(ChatContext ttsContext)
        {
            _context = ttsContext;
        }

        [HttpPost("tts_gen")]
        public async Task<ActionResult<TtsResponse>> TtsGen(Models.Protocol.TtsRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var engine = await _context.TtsEngines.FirstOrDefaultAsync(e => e.Name == request.Engine);
            if (engine == null)
            {
                return NotFound();
            }
            var voice = await _context.TtsVoices.FirstOrDefaultAsync(v => v.Name == request.Voice);
            if (voice == null)
            {
                return NotFound();
            }
            string fileName = $"{user.Id}_{UtilFunc.GetUnixTimeStamp(DateTime.Now)}.mp3";
            string filePath = $"{Defines.StaticVideosPath}/{fileName}";
            TtsService ttsService = new TtsService(engine, voice, request.Text, filePath);
            string voicePath = await ttsService.TtsGen();
            if (string.IsNullOrEmpty(voicePath))
            {
                return BadRequest();
            }
            engine.CallCount++;
            engine.TokenRemain--;
            TtsCall ttsCall = await _context.TtsCalls.Where(c => c.UserId == user.Id && c.VoiceId == voice.Id&& c.EngineId == engine.Id).FirstOrDefaultAsync();
            if (ttsCall == default)
            {
                ttsCall = new()
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    VoiceId = voice.Id,
                    VoiceName = voice.Name,
                    EngineId = engine.Id,
                    EngineName = engine.Name,
                    CallCount = 1,
                };
                await _context.TtsCalls.AddAsync(ttsCall);
            }
            else
            {
                ttsCall.CallCount++;
            }
            await _context.SaveChangesAsync();
            return new TtsResponse
            {
                Code = 0,
                Message = "Success",
                Audio = $"/StaticFiles/Videos/{fileName}"
            };
        }

        [HttpGet("get_engines")]
        public async Task<ActionResult<List<TtsEngine>>> GetEngines()
        {
            return await _context.TtsEngines.ToListAsync();
        }

        [HttpGet("get_voices")]
        public async Task<ActionResult<List<TtsVoice>>> GetVoices()
        {
            return await _context.TtsVoices.ToListAsync();
        }
    }
}
