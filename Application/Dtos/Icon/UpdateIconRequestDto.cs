using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Icon;

public class UpdateIconRequestDto
{
    public Guid Id { get; set; }
    [RegularExpression("^[^\"'!@#%\\^&\\*\\$\\+=~`\\[\\]\\{\\}\\|;:<>\\?\\/,\\\\]{3,45}$",ErrorMessage = "Invalid Characters or Length")]
    public string Name { get; set; } = null!;
    public string DestinationUrl { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public byte Order { get; set; }
}