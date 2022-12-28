using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SolisCloudApi.Client;

public class AuthHeaderService
{
    private const string Verb = "POST";
    private const string BaseUrl = "https://api.soliscloud.com";
    private const string ContentType = "application/json";
    private const string Key = "KeyId";
    private const string KeySecret = "KeySecret";
    private readonly HttpClient _client;

    private readonly ILogger _logger;

    public AuthHeaderService(HttpClient client, ILogger logger)
    {
        this._client = client;
        this._logger = logger;
    }
    /*try {
       String key = "";
       String keySecret = "";
       Map<String,Object> map = new HashMap();
       map.put("pageNo",1);
       map.put("pageSize",10);
       String body = JSON.toJSONString(map);
       String ContentMd5 = getDigest(body);
       String Date = getGMTTime();
       String path = "/v1/api/userStationList";
       String param = "POST" + "\n" + ContentMd5 + "\n" + "application/json" + "\n" + Date + "\n" + path;
       String sign = HmacSHA1Encrypt(param, keySecret);
       String url = "url" + path ;
       OkHttpClient client = new OkHttpClient();
       MediaType xmlType = MediaType.parse("application/json;charset=UTF-8");
       okhttp3.RequestBody requestBody = okhttp3.RequestBody.create(xmlType,body);
       okhttp3.Request request = new okhttp3.Request.Builder()
           .url(url)
           .addHeader("Content-type", "application/json;charset=UTF-8")
           .addHeader("Authorization","API "+key+":"+sign)
           .addHeader("Content-MD5",ContentMd5)
           .addHeader("Date",Date)
           .post(requestBody)
           .build();
       Response response = client.newCall(request).execute();
       String string = response.body().string();
       System.out.println(string);

   } catch (Exception e) {
   e.printStackTrace();
}*/


    public async Task GetStationList(
        CancellationToken cancellationToken)
    {
        var path = "/v1/api/userStationList";
        var body = new UserStationListRequest().ToString();
        var response = await GetApiResponseAsString(path, body, cancellationToken);
    }


    private async Task<string?> GetApiResponseAsString(string path, string body, CancellationToken cancellationToken)
    {
        var contentMd5 = GetDigest(body);
        var date = GetGmtTime();
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
        
        _logger.LogDebug("Sending request to {Url}", fullUrl);

        var httpResponse = await _client.SendAsync(request, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Response: {Response}", responseContent);
            return responseContent;
        }
        else
        {
            _logger.LogInformation("Request failed with status code {StatusCode}", httpResponse.StatusCode);
            return null;
        }
    }

    public string GetAuthHeader(string body, string path = "")
    {
        var contentMd5 = GetDigest(body);
        var date = GetGmtTime();
        path = "/v1/api/userStationList";
        const string contentType = "application/json;charset=UTF-8";
        const string verb = "POST";
        var strings = new[] { verb, contentMd5, contentType, date, path };
        var join = string.Join("\n", strings);
        var param = verb + "\n" + contentMd5 + "\n" + contentType + "\n" + date + "\n" + path;
        var sign = GetHmacSha1(param, KeySecret);
        var authHeader = "API " + Key + ":" + sign;
        return authHeader;
    }

    private static string GetDigest(string body)
    {
        var bytes = Encoding.UTF8.GetBytes(body);
        var hash = MD5.Create().ComputeHash(bytes);
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