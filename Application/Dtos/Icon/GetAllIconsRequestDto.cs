namespace Application.Dtos.Icon;

public class GetAllIconsRequestDto
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}