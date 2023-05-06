using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AliOrdersExtract;

public class OrderExtractor
{
    private static readonly HttpClient httpClient = new();

    public static async Task<List<Order>> ExtractOrdersAsync(string text)
    {
        try
        {
            var orderDatesRegex = new Regex(@"Order date: (\w{3} \d{1,2}, \d{4})");
            var orderIdsRegex = new Regex(@"Order ID: (\d+)");

            var totalAmountsRegex = new Regex(@"Total:US \$(\d+\.\d+)");

            var orderDates = orderDatesRegex.Matches(text);
            var orderIds = orderIdsRegex.Matches(text);

            var totalAmounts = totalAmountsRegex.Matches(text);

         

         

            var orders = new List<Order>();

            if (orderDates.Count == 0 || orderIds.Count == 0 || totalAmounts.Count == 0  )
            {
                // No orders found in the text
                return orders;
            }

            if (  orderIds.Count != totalAmounts.Count)
            {
                // Mismatch in the counts of orderIds, productNames, and totalAmounts
                // Handle the error or throw an exception as needed
                throw new Exception("Mismatch in the counts of orderIds, productNames, and totalAmounts.");
            }

            for (var i = 0; i < orderIds.Count; i++)
            {
                var exchangeRate = await GetExchangeRateAsync(DateTime.Parse(orderDates[i].Groups[1].Value));

                var order = new Order
                {
                    OrderDate = DateTime.Parse(orderDates[i].Groups[1].Value),
                    OrderID = orderIds[i].Groups[1].Value,
                   
                    TotalUSD = decimal.Parse(totalAmounts[i].Groups[1].Value),
                    TotalILS = decimal.Parse(totalAmounts[i].Groups[1].Value) * exchangeRate
                };
                orders.Add(order);
            }

            return orders;
        }
        catch (Exception ex)
        {
            // Handle the exception (e.g., log the error, display a message, etc.)
            Console.WriteLine($"An error occurred while extracting orders: {ex.Message}");
            return new List<Order>(); // Return an empty list to indicate no orders found or error occurred
        }
    }



    private static async Task<decimal> GetExchangeRateAsync(DateTime date)
    {
        string accessKey = "API_KEY"; // Replace with your access key
        string apiUrl = $"https://api.apilayer.com/exchangerates_data/convert?to=ILS&from=USD&amount=1&apikey={accessKey}";
        var client = new HttpClient();
        string response = await client.GetStringAsync(apiUrl);

        JObject json = JObject.Parse(response);
        decimal exchangeRate = json["result"].Value<decimal>();

        return exchangeRate;
    }

    

}