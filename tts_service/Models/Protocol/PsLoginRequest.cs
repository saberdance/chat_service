
namespace tts_service.Models.Protocol
{
    public class PsLoginRequest : BaseRequest
    {
        public string? Account { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }

        public bool Validate()
        {
            return !(string.IsNullOrEmpty(Account) && string.IsNullOrEmpty(Phone)) 
                && !string.IsNullOrEmpty(Password);
        }
    }
}
