using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AliOrdersExtract;

public class OrderExtractorApp
{
    public async Task RunAsync(string inputFilePath)
    {
        string text = File.ReadAllText(inputFilePath);
        var orders = await OrderExtractor.ExtractOrdersAsync(text);

        string csvFileName = "orders.csv";
        await SaveOrdersToCSV(orders, csvFileName);

        Console.WriteLine($"Order information saved to {csvFileName}");
    }

    private static async Task SaveOrdersToCSV(List<Order> orders, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            // Write CSV header
            await writer.WriteLineAsync("Order Date,Order ID,Total (USD),Total (ILS)");

            // Write order information
            foreach (var order in orders)
            {
                string orderDate = order.OrderDate.ToString("yyyy-MM-dd");
                string totalUSD = order.TotalUSD.ToString(CultureInfo.InvariantCulture);
                string totalILS = order.TotalILS.ToString(CultureInfo.InvariantCulture);

                string csvLine = $"{orderDate},{order.OrderID} ,{order.Quantity},{totalUSD},{totalILS}";
                await writer.WriteLineAsync(csvLine);
            }
        }
    }
}