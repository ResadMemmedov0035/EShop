namespace EShop.Application.Security.Token;

public class TokenOptions
{
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string SecurityKey { get; set; } = string.Empty;
    public int Lifetime { get; set; } // minute
};
