namespace Application.Dtos.Icon;

public class UpdateIconRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string DestinationUrl { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public byte Order { get; set; }
}