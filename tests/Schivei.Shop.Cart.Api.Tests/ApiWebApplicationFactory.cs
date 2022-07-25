namespace Schivei.Shop.Cart.Api.Tests;

internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { });
        builder.ConfigureTestServices(services => { });
    }
}

public class AppFactory
{
    private readonly ApiWebApplicationFactory _factory;
    public AppFactory() => _factory = new();

    public HttpClient CreateClient() => _factory.CreateClient();
}

