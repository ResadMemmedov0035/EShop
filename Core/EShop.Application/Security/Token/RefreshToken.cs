namespace EShop.Application.Security.Token;

public record RefreshToken(string Token, DateTime Expiration);
