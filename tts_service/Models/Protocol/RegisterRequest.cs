namespace tts_service.Models.Protocol
{
    public enum AccountType
    {
        Password,
        Phone,
        WeiXin,
        Email
    }
    public class RegisterRequest
    {
        public AccountType AccountType { get; set; } = AccountType.Password;
        public string? Account { get; set; } = "";
        public string? Username { get; set; } = "";
        public string? Password { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string? WeiXinAccount { get; set; }

        public bool Validate()
        {
            if (AccountType == AccountType.Password)
            {
                return !string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(Password);
            }
            else if (AccountType == AccountType.Phone)
            {
                return !string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(Phone);
            }
            else if (AccountType == AccountType.WeiXin)
            {
                return !string.IsNullOrEmpty(WeiXinAccount);
            }
            else if (AccountType == AccountType.Email)
            {
                return !string.IsNullOrEmpty(Email);
            }
            return false;
        }
    }
}
