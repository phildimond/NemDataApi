namespace NemDataApi;

using System.Net;

public class Network
{
    // ====================================================
    // Perform a GET API transaction
    // ====================================================

    public static (HttpStatusCode statusCode, string responseBody) ProcessRequest(string url)
    {
        var clientHandler = new HttpClientHandler
        {
            UseCookies = false,
        };

        var client = new HttpClient(clientHandler);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };

        using (var response = client.Send(request))
        {
            var result = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(result);
            return (response.StatusCode, reader.ReadToEnd());
        } 
    }
}