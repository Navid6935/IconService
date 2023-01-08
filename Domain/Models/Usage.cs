namespace Domain.Models;

public class Usage : BaseModel
{
    public Guid IconId { get; set; }
    public string Username { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public int Count { get; set; }

    public Icon Icon { get; set; } = null!;
}