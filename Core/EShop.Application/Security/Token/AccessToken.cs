namespace EShop.Application.Security.Token;

public record AccessToken(string Token, DateTime Expiration);
