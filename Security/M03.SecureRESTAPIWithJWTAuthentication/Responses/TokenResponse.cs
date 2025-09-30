namespace M03.SecureRESTAPIWithJWTAuthentication.Responses;

public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}