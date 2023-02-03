namespace Application.Dtos.Page;

public class AddPageRequestDto
{
    public string Name { get; set; } = null!;
    public Guid PageId { get; set; }
}