namespace Application.DTOs.Internal;
public class RedisServiceDTO
{
    public string Key { get; set; } = null!;
    public string? Value { get; set; } = null!;
    public bool IsPassed { get; set; }
    public string? OldValue { get; set; }
    public RedisType Type { get; set; }
}

public enum RedisType
{
    Set = 1,
    Delete = 2,
    Update = 3
}

