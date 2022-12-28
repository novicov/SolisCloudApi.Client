namespace SolisCloudApi.Client;

public class ApiReponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Msg { get; set; }
    public object Data { get; set; }
}