using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Usage;

public class AddUsageRequestDto
{
    public Guid IconId { get; set; }
    [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b",ErrorMessage = "Invalid Ip Address")]
    public string Ip { get; set; } = null!;
    public int Count { get; set; }
}