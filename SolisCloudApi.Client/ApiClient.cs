using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SolisCloudApi.Client;

public class ApiClient
{
    private const string Verb = "POST";
    private const string BaseUrl = "https://api.soliscloud.com";
    private const string ContentType = "application/json";
    private const string Key = "2424";
    private const string KeySecret = "6680182547";
    private readonly HttpClient _client;

    private readonly ILogger _logger;

    public ApiClient(HttpClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task GetStationList(
        CancellationToken cancellationToken)
    {
        var path = "/v1/api/userStationList";
        var body = new UserStationListRequest();
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonBody = JsonSerializer.Serialize(body, jsonOptions);

        try
        {
            var response = await GetApiResponse(path, jsonBody, cancellationToken);
            if (response?.Data is not null && response.Success)
            {
                var result = JsonSerializer.Deserialize<UserStationsResponse>(response.Data, jsonOptions);
                _logger.LogInformation("Station info: {StationData}", result);
            }
            else
            {
                _logger.LogInformation("Station data is null. Reason: {Reason}", response?.Msg);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Send request to SolisCloud failed");
        }
    }


    private async Task<ApiResponse?> GetApiResponse(string path, string body,
        CancellationToken cancellationToken)
    {
        var contentMd5 = GetDigest(body);
        var date = GetGmtTime();
        // just for debugging:
        // var date = "Fri, 26 Jul 2019 06:00:46 GMT";
        string[] paramsToSign = { Verb, contentMd5, ContentType, date, path };
        var paramsAsString = string.Join("\n", paramsToSign);
        var sign = GetHmacSha1(paramsAsString, KeySecret);
        var fullUrl = BaseUrl + path;
        var authHeader = "API " + Key + ":" + sign;
        var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
        request.Content = new StringContent(body, Encoding.UTF8, ContentType);
        request.Headers.Add("Authorization", authHeader);
        request.Headers.Add("Date", date);
        request.Content.Headers.Add("Content-MD5", contentMd5);

        _logger.LogInformation("Sending request to {Url}", fullUrl);

        var httpResponse = await _client.SendAsync(request, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            // just for debugging:
            // var strResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var responseContent =
                await httpResponse.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken: cancellationToken);
            _logger.LogInformation("Response: {Response}", responseContent);
            if (responseContent is not null)
            {
                return responseContent;
            }
        }
        else
        {
            _logger.LogInformation("Request failed with status code {StatusCode}", httpResponse.StatusCode);
        }

        return null;
    }

    private static string GetDigest(string body)
    {
        var bytes = Encoding.UTF8.GetBytes(body);
        var hash = MD5.HashData(bytes);
        var digest = Convert.ToBase64String(hash);
        return digest;
    }

    private static string GetGmtTime()
    {
        var dt = DateTime.UtcNow;
        var date = dt.ToString("r");
        return date;
    }

    private static string GetHmacSha1(string data, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var dataBytes = Encoding.UTF8.GetBytes(data);
        using var hmac = new HMACSHA1(keyBytes);
        var hashBytes = hmac.ComputeHash(dataBytes);
        return Convert.ToBase64String(hashBytes);
    }
}