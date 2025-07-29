namespace WAPI_GS.Service
{
    public class KeepAliveService : BackgroundService
    {
        private readonly ILogger<KeepAliveService> _logger;
        private readonly HttpClient _httpClient;

        public KeepAliveService(ILogger<KeepAliveService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var url = "https://wapi-sistema-gestao-salas.onrender.com/api/v1/atribuicoes/imhere";

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url, stoppingToken);
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("KeepAlive: Sucesso ao chamar imhere.");
                    }
                    else
                    {
                        _logger.LogWarning("KeepAlive: Falha ao chamar imhere. Status: {StatusCode}", response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "KeepAlive: Erro ao chamar imhere.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
