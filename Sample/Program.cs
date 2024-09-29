using NemDataApi;

namespace Sample;

/*
    AusGrid EA030:
    The export tariff code is EA029 and will apply to applicable customers as a secondary tariff.
    The tariff includes both a charge and reward/rebate price that will apply to net metered ‘B
    channel’ energy, as dependent on the time of day. The charge applies between 10am and
    3pm every day and the reward/rebate applies between 4pm and 9pm every day.
    
    The charge component includes a free threshold where part of the total exported energy in a
    billing period is exempt from receiving a charge. The amount of the free threshold is 6.85 
    kWh per number of days in the billing period. For monthly billed customers the amount of
    the energy exempt from the charge is as follows:
    • 212.4 kWh for 31 day months (January, March, May, July, August, October, December)
    • 205.5 kWh for 30 day months (April, June, September, November)
    • 198.7 kWh for 29 day months (February in a leap year)
    • 191.8 kWh for 28 day months (February in all other years)
   
    Charges (FY 2024-2025): 1.2029c/kWh charge and -2.3951c/kWh reward
    
 */
class Program
{
    private const decimal AmberBuyNetworkCharges = (decimal)8.05; //Amber c/kWh charge
    private const decimal AmberSellNetworkCharges = (decimal)8.05; //Amber c/kWh charge
    private const decimal GstMultiplier = (decimal)1.1; // GST multiplier

    static void Main(string[] args)
    {
        NemDataConnection nemConnection = new NemDataConnection();
        
        NemPriceRecord[]? recs = nemConnection.GetNemPriceRecords("NSW1", DateTime.Today);
        
        if (recs == null || recs.Length == 0)
        {
            Console.WriteLine("No Nem Price Records Found");
            return;
        }

        DateTime lastActualPriceTime = DateTime.MinValue;
        for (int i = 0; i < recs.Length; i++)
        {
            decimal averagePrice = 0;

            Console.Write($"{recs[i].NemDateTime:MM/dd/yyyy} ");

            if (recs[i].IsActualPrice)
            {
                Console.Write($"{(recs[i].NemDateTime.AddMinutes(-5)):HH:mm:ss} to {recs[i].NemDateTime:HH:mm:ss}: ");
                if (i > 5)
                {
                    averagePrice = 0;
                    for (int j = i - 5; j <= i; j++) averagePrice += recs[j].PricePerMwh;
                    averagePrice /= 6;
                }

                lastActualPriceTime = recs[i].NemDateTime;
            }
            else // forecast
            {
                int addMins = -30;
                if (lastActualPriceTime != DateTime.MinValue)
                {
                    addMins = 0 - (30 - lastActualPriceTime.Minute);
                    if (addMins > 30) addMins -= 30;
                }

                Console.Write(
                    $"{recs[i].NemDateTime.AddMinutes(addMins):HH:mm:ss} to {(recs[i].NemDateTime):HH:mm:ss}: ");
                lastActualPriceTime = DateTime.MinValue;
            }

            Console.Write(recs[i].IsActualPrice ? "Actual " : "Forecast ");
            decimal priceCentsPerKwh = (recs[i].PricePerMwh / 1000 * 100); // MWh->kWh div 1000, $ to cents * 100
            priceCentsPerKwh += AmberBuyNetworkCharges;
            priceCentsPerKwh *= GstMultiplier;
            Console.Write($"{(priceCentsPerKwh):0.0000}c/kWh ");

            if (recs[i].IsActualPrice)
            {
                decimal averagePriceCentsPerKwh =
                    averagePrice / 1000 * 100; // MWh->kWh div 1000, $ to cents * 100
                averagePriceCentsPerKwh += AmberBuyNetworkCharges;
                averagePriceCentsPerKwh *= GstMultiplier;
                Console.Write($"Average = {averagePriceCentsPerKwh:0.0000}c/kWh ");
            }

            Console.WriteLine();
        }
    }
}