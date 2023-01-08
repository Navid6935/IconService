namespace Application.Dtos.Usage;

public class AddUsageRequestDto
{
    public Guid IconId { get; set; }
    public string Username { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public int Count { get; set; }
}