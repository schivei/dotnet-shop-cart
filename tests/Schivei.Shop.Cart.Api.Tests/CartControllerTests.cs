using FluentAssertions;
using Newtonsoft.Json;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Models;
using System.Net;
using System.Text;

namespace Schivei.Shop.Cart.Api.Tests;

public class CartControllerTests : IClassFixture<AppFactory>
{
    private readonly HttpClient _client;

    public CartControllerTests(AppFactory fixture) => _client = fixture.CreateClient();

    private static IEnumerable<string[]> Guids(int size)
    {
        if (size <= 0)
        {
            yield return new string[] { default! };
            yield break;
        }

        yield return Enumerable.Range(0, size).Select(x => Guid.NewGuid().ToString()).ToArray();
    }

    [Fact]
    public async Task GET_cart() // GET /cart
    {
        var response = await _client.GetAsync("/cart");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.Id.Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task GET_cart_cartId_resume(string cartId)
    {
        var response = await _client.GetAsync($"/cart/{cartId}/resume");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resume = await response.Content.ReadAsJsonAsync<CartResumeResponseVM>();

        resume.Should().NotBeNull();
        resume.Subtotal.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task GET_cart_cartId(string cartId)
    {
        var response = await _client.GetAsync($"/cart/{cartId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task GET_cart_user_userId(string userId)
    {
        var response = await _client.GetAsync($"/cart/user/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.Id.Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Guids), 2)]
    public async Task PATCH_cart_cartId_user(string cartId, string userId)
    {
        var request = new CartSetUserRequestVM
        {
            UserId = new Guid(userId)
        };
        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"/cart/{cartId}/user", json);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await _client.GetAsync($"/cart/{cartId}/resume");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resume = await response.Content.ReadAsJsonAsync<CartResumeResponseVM>();

        resume.Should().NotBeNull();
        resume.UserId.Should().Be(request.UserId);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task PUT_cart_cartId(string cartId)
    {
        var request = new CartItemUpdateRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Value.Sku
        };

        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/cart/{cartId}", json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.Id.Should().Be(new Guid(cartId));
    }

    [Theory]
    [MemberData(nameof(Guids), 0)]
    [MemberData(nameof(Guids), 1)]
    public async Task POST_cart(string? userId = null)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = userId is null ? null : new Guid(userId)
        };

        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cart", json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task DELETE_cart_cartId_sku(string userId)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = new Guid(userId)
        };

        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cart", json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().NotBeEmpty();

        var cartId = content.Id;

        response = await _client.DeleteAsync($"/cart/{cartId}/{request.Sku}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await _client.GetAsync($"/cart/{cartId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task DELETE_cart_cartId(string userId)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = new Guid(userId)
        };

        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cart", json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().NotBeEmpty();

        var cartId = content.Id;

        response = await _client.DeleteAsync($"/cart/{cartId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await _client.GetAsync($"/cart/{cartId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content = await response.Content.ReadAsJsonAsync<CartResponseVM>();

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().BeEmpty();
    }
}
