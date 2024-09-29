# NemDataApi
 
This C# DLL and sample program uses an undocumented AEMO open API to retrieve prices for the spot market.

The API calls were gleaned by inspection of the Dashboard web page. I'm glad these are there, as they make trading via Amber and similar retailer much easier.

The sample program uses the Price and Demand data to calculate the settlement and forecast prices from the current day, adding in the network tariff charges for the Ausgrid EA116 Residential Demand tariff. This cna be compared to Amber Electric prices. Amber now seems to be using their own "forecast" prices instead of the AEMO ones, and they're rubbish from my experience.  

The APIs I've identified to date include:

Data from the "Price and Demand dashboard": https://visualisations.aemo.com.au/aemo/apps/api/report/NEM_DASHBOARD_CUMUL_PRICE

Data from the Dispatch Overview page:
https://visualisations.aemo.com.au/aemo/apps/api/report/ELEC_NEM_SUMMARY

Data from the Cumulative Price page: https://visualisations.aemo.com.au/aemo/apps/api/report/NEM_DASHBOARD_MARKET_PRICE_LIMITS

Data from the Fuel Mix page:
https://visualisations.aemo.com.au/aemo/apps/api/report/FUEL

Data from the Renewable Penetration page: https://visualisations.aemo.com.au/aemo/apps/api/report/FUEL

Data from the Average Price page. The csv file name lets you pick the month and year (eg..._DAY_YYYYMM.csv): https://visualisations.aemo.com.au/aemo/data/nem/averageprices/WEB_AVERAGE_PRICE_DAY_202409.csv

Data from the 7-Day Outlook Page. The query is a date field, probably a Unix date, but I haven't bothered looking: https://visualisations.aemo.com.au/aemo/data/NEM/SEVENDAYOUTLOOK/WEB_SEVENDAY_OUTLOOK.CSV?d=1727581926923

