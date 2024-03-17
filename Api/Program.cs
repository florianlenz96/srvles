using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient<SvrlesClient>(client =>
        {
            client.BaseAddress = new Uri("https://srvlesurlshortener.azure-api.net");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9f0a307d9ee2478a9725dfd44ac959ed");
        });
    })
    .Build();

host.Run();
