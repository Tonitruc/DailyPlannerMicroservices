namespace UserService.Dtos;

public class BaerarTokenDto()
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpireRefreshToken { get; init; } 
}
