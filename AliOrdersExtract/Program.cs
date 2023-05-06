using System.Threading.Tasks;
using AliOrdersExtract;

class Program
{
    static async Task Main(string[] args)
    {
        string inputFilePath = "input.txt"; // Replace with the path to your input text file

        OrderExtractorApp app = new OrderExtractorApp();
        await app.RunAsync(inputFilePath);
    }
}