namespace Application.DTOs;

public sealed class ResponseDTO
{

    public int StatusCode { get; set; }
    public List<string> Message { get; set; } = null!;
    public int Total { get; set; } = 0;
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
    public object? Result { get; set; }
}
