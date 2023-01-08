namespace Application.Dtos.Usage.Response;

public class UsageResponseDto
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
    public string Username { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public int Count { get; set; }
}