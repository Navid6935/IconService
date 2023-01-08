namespace Application.Dtos.Usage;

public class AddUsageRequestDto
{
    public Guid IconId { get; set; }
    public string Ip { get; set; } = null!;
    public int Count { get; set; }
}