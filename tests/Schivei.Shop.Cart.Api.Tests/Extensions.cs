using Newtonsoft.Json;

namespace Schivei.Shop.Cart.Api.Tests;

public static class Extensions
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent content)
    {
        var str = await content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T?>(str);
    }
}
