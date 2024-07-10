using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tsubasa;
using tts_service.Db;
using tts_service.Models;
using tts_service.Models.Protocol;
using tts_service.Models.Session;
using tts_service.Services;

namespace tts_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ChatContext _context;
        private readonly JwtHelper _jwtHelper;
        public UserController(ChatContext context,JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<User>>> Register(RegisterRequest request)
        {
            if (!request.Validate())
            {
                return BadRequest(new InvalidParamResponse());
            }
            return request.AccountType switch
            {
                AccountType.Password => await RegisterPassword(request),
                AccountType.Phone => await RegisterPhone(request),
                AccountType.WeiXin => await RegisterWeiXin(request),
                AccountType.Email => await RegisterEmail(request),
                _ => BadRequest(new InvalidParamResponse("Invalid account type" ))
            };
        }
        

        private async Task<ActionResult<BaseResponse<User>>> RegisterEmail(RegisterRequest request)
        {
            //TODO：邮箱注册
            return NotFound();
        }

        private async Task<ActionResult<BaseResponse<User>>> RegisterWeiXin(RegisterRequest request)
        {
            //TODO：微信号注册
            return NotFound();
        }

        private async Task<ActionResult<BaseResponse<User>>> RegisterPhone(RegisterRequest request)
        {
            //手机号账户注册
            return NotFound();

        }

        private async Task<ActionResult<BaseResponse<User>>> RegisterPassword(RegisterRequest request)
        {
            //密码账户注册
            if (await _context.Users.AnyAsync(u => u.Account == request.Account))
            {
                return BadRequest(new InvalidCredentialResponse("Account already exists"));
            }
            var user = new User
            {
                Account = request.Account,
                Password = UtilFunc.MD5String(request.Password),
                Username = request.Username,
                Phone = request.Phone,
                Role = UserRole.User
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new SuccessResponse<User>(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(PsLoginRequest request)
        { 
            if (!request.Validate())
            {
                return BadRequest(new InvalidCredentialResponse("Invalid Login Param"));
            }
            User user = null;
            if (!string.IsNullOrEmpty(request.Account))  
            {
                user = await _context!.Users!.FirstOrDefaultAsync(u => u.Account == request.Account && u.Password == UtilFunc.MD5String(request.Password));
            }
            else
            {
                user = await _context!.Users!.FirstOrDefaultAsync(u => u.Phone == request.Phone && u.Password == UtilFunc.MD5String(request.Password));
            }
            if (user == null)
            {
                return BadRequest(new InvalidCredentialResponse("Invalid Account or Password"));
            }
            if (DateTime.Now < user.TokenExpire && !string.IsNullOrEmpty(user.Token))
            {
            }
            else
            {
                user.Token = _jwtHelper.CreateToken();
                user.TokenExpire = DateTime.Now.AddDays(30);
                await _context.SaveChangesAsync();
            }
            return Ok(new BaseResponse<LoginResponse>
            {
                Code = 0,
                Message = "Login Success",
                Data = new() {Id= user.Id, Guid = user.Guid, Token = user.Token, TokenExpire = user.TokenExpire }
            });
        }
    }
}