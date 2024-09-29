using System.Net;
using System.Text.Json;

namespace NemDataApi;

public class NemDataConnection
{
    private string _url = "https://visualisations.aemo.com.au/aemo/apps/api/report/NEM_DASHBOARD_CUMUL_PRICE";

    /// <summary>
    /// Constructor
    /// </summary>
    public NemDataConnection()
    {
        
    }

    /// <summary>
    /// Get the current NEM dash board price records
    /// </summary>
    /// <returns></returns>
    public NemPriceRecord[]? GetNemPriceRecords()
    {
        (HttpStatusCode statusCode, string responseBody) responseData;

        responseData = Network.ProcessRequest(_url);
        if (responseData.statusCode != HttpStatusCode.OK) return null;
        NemPriceDataSet? data = JsonSerializer.Deserialize<NemPriceDataSet>(responseData.responseBody);

        if (data == null) return null;
        if (data.NemPriceRecords == null) return null;
        
        return data.NemPriceRecords;
    }
}