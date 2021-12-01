using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HealthLifeCheckService
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HealthCheckService> _logger;

        public HealthCheckService(HttpClient httpClient, ILogger<HealthCheckService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> Check(string url)
        {
            HttpResponseMessage checkingResponse;
            try
            {
                checkingResponse = await _httpClient.GetAsync(new Uri(url));
            } 
            catch (Exception ex)
            {

                _logger.LogError($"Error happened while checking the url {url} health.");
                throw;
            }

             _logger.LogInformation($"Health check of {url} is {(checkingResponse.IsSuccessStatusCode ? "Success": "Failed")}");


            return checkingResponse.IsSuccessStatusCode;
        }
    }
}
