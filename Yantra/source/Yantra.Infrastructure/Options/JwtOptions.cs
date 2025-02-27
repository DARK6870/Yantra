namespace Yantra.Infrastructure.Options;

public class JwtOptions
{
    public required string SecretKey { get; set; }

    public int ExpireInMinutes { get; set; } = 60;
}