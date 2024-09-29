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
    /// <param name="region">NEM region: NSW1, QLD1, VIC1, TAS1, SA1 </param>
    /// <param name="date">Calendar ate that records are desired for</param>
    /// <returns>null on error or no data, array of NemPriceRecords othewise</returns>
    public NemPriceRecord[]? GetNemPriceRecords(string? region = null, DateTime? date = null)
    {
        (HttpStatusCode statusCode, string responseBody) responseData;

        responseData = Network.ProcessRequest(_url);
        if (responseData.statusCode != HttpStatusCode.OK) return null;
        NemPriceDataSet? data = JsonSerializer.Deserialize<NemPriceDataSet>(responseData.responseBody);

        if (data == null) return null;
        if (data.NemPriceRecords == null) return null;
        if (data.NemPriceRecords.Length == 0) return null;

        if (region != null)
            data.NemPriceRecords = (from r in data.NemPriceRecords where (r.Region == region) select r).ToArray();

        if (date.HasValue)
            data.NemPriceRecords =
                (from r in data.NemPriceRecords where (r.NemDateTime.Date == date.Value.Date) select r).ToArray();

        return data.NemPriceRecords;
    }
}