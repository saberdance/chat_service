namespace tts_service.Models.Protocol
{
    public class SuccessResponse<T> : BaseResponse<T>
    {
        public SuccessResponse(T data)
        {
            Code = ProtocolErrorCode.Success;
            Message = "";
            Data = data;
        }
    }

    public class UserNotFoundResponse : BaseResponse<string> 
    { 
        public UserNotFoundResponse(string? msg = null)
        {
            Code = ProtocolErrorCode.InvalidCredential;
            Message = msg ?? "User not found";
            Data = null;
        }
    }

    public class InvalidCredentialResponse : BaseResponse<string>
    {
        public InvalidCredentialResponse(string? msg = null)
        {
            Code = ProtocolErrorCode.InvalidCredential;
            Message = msg ?? "Invalid credential";
            Data = null;
        }
    }

    public class InvalidParamResponse : BaseResponse<string>
    {
        public InvalidParamResponse(string? msg = null)
        {
            Code = ProtocolErrorCode.InvalidParameter;
            Message = msg ?? "Invalid param";
            Data = null;
        }
    }

    public class InvalidTokenResponse : BaseResponse<string>
    {
        public InvalidTokenResponse(string? msg = null)
        {
            Code = ProtocolErrorCode.InvalidToken;
            Message = msg ?? "Invalid token";
            Data = null;
        }
    }
}
