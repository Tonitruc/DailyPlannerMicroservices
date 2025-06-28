namespace UserService.UseCases.Services.Authentication;

public class JwtTokensSettings
{
    public required JwtTokenOptions ActiveToken {  get; init; }
    public required JwtTokenOptions RefreshToken { get; init ; }
}

public class JwtTokenOptions
{
    public int Expire { get; init; }
    public string SecurityKey { get; init; } = string.Empty;
}
