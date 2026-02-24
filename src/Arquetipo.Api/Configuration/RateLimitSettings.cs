namespace Arquetipo.Api.Configuration;

public class RateLimitSettings
{
    public string Policy { get; set; } = "Fixed";
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
    public int QueueLimit { get; set; }
}
