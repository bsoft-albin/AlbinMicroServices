using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlbinMicroService.Libraries.BuildingBlocks.BackgroundServices
{
    public class WarmUpService(IHttpClientFactory clientFactory, ILogger<WarmUpService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Give downstream services time to finish booting and Delay slightly to allow services to be ready
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            string[] warmUpUrls = [
                "http://localhost:8001/readiness",
                "http://localhost:8003/readiness",
                "http://localhost:8005/readiness"
            ];

            var client = clientFactory.CreateClient();

            foreach (var url in warmUpUrls)
            {
                try
                {
                    var response = await client.GetAsync(url, stoppingToken);
                    logger.LogInformation($"Warmed up {url} - Status: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Warm-up failed for {url}");
                }
            }
        }
    }
}
