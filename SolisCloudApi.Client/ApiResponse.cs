namespace SolisCloudApi.Client;

public record ApiResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string? Msg { get; set; }
    public string? Data { get; set; }
}