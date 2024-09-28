using System.Text.Json.Serialization;

namespace NemDataApi;

public class NemPriceDataSet
{
    [JsonPropertyName("NEM_DASHBOARD_CUMUL_PRICE")]
    public NemPriceRecord[]? NemPriceRecords { get; set; } = null;
}

/// <summary>
/// Encapsulates a NEM dashboard spot price API record
/// </summary>
public class NemPriceRecord
{
    /// <summary>
    /// NEM period END DateTime of this record
    /// </summary>
    [JsonPropertyName("DT")]
    public DateTime NemDateTime { get; set; }

    /// <summary>
    /// Price per MHw in DOLLARS
    /// </summary>
    [JsonPropertyName("P")] 
    public decimal PricePerMwh { get; set; }

    /// <summary>
    /// Cumulative restriction price in DOLLARS
    /// </summary>
    [JsonPropertyName("CP")] 
    public double CumulativePrice { get; set; }

    /// <summary>
    /// NEM region: NSW1, QLD1, SA1, TAS1, VIC1
    /// </summary>
    [JsonPropertyName("R")] 
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Is this an actual or forecast price? Actual = 1, otherwise a forecast
    /// </summary>
    [JsonPropertyName("A")] 
    public int Actual { get; set; }

    /// <summary>
    /// Is this an actual or forecast price?
    /// </summary>
    public bool IsActualPrice
    {
        get
        {
            if (Actual == 0) return false;
            else return true;
        }
    }
}