namespace Application.Dtos.Usage;

public class GetUsagesByUsernameRequestDto
{
    public string Username { get; set; } = null!;
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}