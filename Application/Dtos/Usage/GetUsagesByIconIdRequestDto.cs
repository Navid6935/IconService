namespace Application.Dtos.Usage;

public class GetUsagesByIconIdRequestDto
{
    public Guid IconId { get; set; }
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}