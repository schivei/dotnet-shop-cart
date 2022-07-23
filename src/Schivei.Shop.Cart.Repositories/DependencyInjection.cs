using Schivei.Shop.Cart.Infrastructure.Repositories;
using Schivei.Shop.Cart.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddRepos(this IServiceCollection services) =>
        services.AddScoped<ICheckoutRepository, CheckoutRepository>()
            .AddScoped<ICartRepository, CartRepository>();
}
