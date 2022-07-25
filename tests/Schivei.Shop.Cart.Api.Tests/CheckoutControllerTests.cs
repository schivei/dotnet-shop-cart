using FluentAssertions;
using Newtonsoft.Json;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Models;
using System.Net;
using System.Text;

namespace Schivei.Shop.Cart.Api.Tests;

public class CheckoutControllerTests : IClassFixture<AppFactory>
{
    private readonly HttpClient _client;

    public CheckoutControllerTests(AppFactory fixture) => _client = fixture.CreateClient();

    private static IEnumerable<string[]> Guids(int size)
    {
        yield return Enumerable.Range(0, size).Select(x => Guid.NewGuid().ToString()).ToArray();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task POST_checkout_userId(string userId)
    {
        var product = DataMocking.SKUs.First().Value;

        var requestCart = new CreateCartWithItemsRequestVM
        {
            Quantity = 3,
            Sku = product.Sku,
            UserId = new Guid(userId)
        };

        var json = new StringContent(JsonConvert.SerializeObject(requestCart), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cart", json);

        var cart = await response.Content.ReadAsJsonAsync<CartResponseVM>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        cart.Items.Should().HaveCount(1);
        cart.Items.First().Quantity.Should().Be(3);

        var request = new CheckoutRequestVM
        {
            ZipCode = "13530000",
            Coupon = "OFF15"
        };

        json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        response = await _client.PostAsync($"/checkout/{userId}", json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var checkout = await response.Content.ReadAsJsonAsync<CheckoutResponseVM>();

        var zipTax = 1.3m;
        var sum = product.SellingPrice * 3;
        var couponDiscout = 0.15m * sum;
        var total = sum - couponDiscout + zipTax;

        checkout.Delivery.Should().Be(zipTax);
        checkout.Discount.Should().Be(couponDiscout);
        checkout.Subtotal.Should().Be(sum);
        checkout.Total.Should().Be(total);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task PUT_checkout_userId(string userId)
    {
        await POST_checkout_userId(userId);

        var request = new CheckoutDoPaymentRequestVM
        {
            CardName = "TEST CARD",
            CardNumber = "1234567890123456",
            CvcCode = "123"
        };

        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/checkout/{userId}", json);

        var cart = await response.Content.ReadAsJsonAsync<CartResponseVM>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        cart.UserId.Should().Be(new Guid(userId));
        cart.Items.Should().BeEmpty();
    }
}
