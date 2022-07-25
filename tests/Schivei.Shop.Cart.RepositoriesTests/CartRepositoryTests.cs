using FluentAssertions;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Models;
using Xunit;

namespace Schivei.Shop.Cart.Repositories.Tests;

public class CartRepositoryTests
{
    private readonly CartRepository _repository;

    public CartRepositoryTests() => _repository = new();

    private static IEnumerable<string[]> Guids(int size)
    {
        if (size <= 0)
        {
            yield return new string[] { default! };
            yield break;
        }

        yield return Enumerable.Range(0, size).Select(x => Guid.NewGuid().ToString()).ToArray();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task AddOrUpdateCartItemTest(string cartId)
    {
        CartId cid = new Guid(cartId);

        var request = new CartItemUpdateRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Value.Sku
        };

        var response = await _repository.AddOrUpdateCartItem(cid, request);
        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        var content = response.Item1!.Value;

        content.Should().NotBeNull();
        content.Id.Should().Be(new Guid(cartId));
        content.Items.Should().NotBeEmpty();
        content.Items.First().Quantity.Should().Be(request.Quantity);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task ClearTest(string userId)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = new Guid(userId)
        };

        var response = await _repository.Create(request);

        response.Item1.Should().NotBeNull();

        var cartId = response.Item1!.Value.Id;

        var except = await _repository.Clear(cartId);
        except.Should().BeNull();

        response = await _repository.OpenCart(cartId);
        response.Item1.Should().NotBeNull();

        var content = response.Item1!.Value;

        content.Id.Should().Be(cartId);
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateTest()
    {
        var response = await _repository.Create();

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task CreateTest1(string userId)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = new Guid(userId)
        };

        var response = await _repository.Create(request);

        response.Item1.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task DeleteItemBySKUTest(string userId)
    {
        var request = new CreateCartWithItemsRequestVM
        {
            Quantity = 1,
            Sku = DataMocking.SKUs.First().Key,
            UserId = new Guid(userId)
        };

        var response = await _repository.Create(request);

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        var content = response.Item1!.Value;

        content.Should().NotBeNull();
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().NotBeEmpty();

        var cartId = content.Id;

        var except = await _repository.DeleteItemBySKU(cartId, request.Sku);
        except.Should().BeNull();

        response = await _repository.OpenCart(cartId);
        response.Item1.Should().NotBeNull();

        content = response.Item1!.Value;

        content.Id.Should().Be(cartId);
        content.UserId.Should().Be(request.UserId);
        content.Items.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task ResumeTest(string cartId)
    {
        var cid = new Guid(cartId);

        var response = await _repository.Resume(new Guid(cartId));

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        response.Item1!.Value.Id.Should().Be(cid);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task OpenCartTest(string cartId)
    {
        var cid = new Guid(cartId);

        var response = await _repository.OpenCart(cid);

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        response.Item1!.Value.Id.Should().Be(cid);
    }

    [Theory]
    [MemberData(nameof(Guids), 1)]
    public async Task ReOpenTest(string userId)
    {
        var uid = new Guid(userId);

        var response = await _repository.ReOpen(uid);

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        response.Item1!.Value.UserId.Should().Be(uid);
    }

    [Theory]
    [MemberData(nameof(Guids), 2)]
    public async Task SetUserTest(string cartId, string userId)
    {
        var cid = new Guid(cartId);
        var uid = new Guid(userId);

        var response = await _repository.SetUser(cid, new() { UserId = uid });

        response.Should().BeNull();

        var (cart, _) = await _repository.OpenCart(cid);

        cart.Should().NotBeNull();

        cart!.Value.Id.Should().Be(cid);
        cart!.Value.UserId.Should().Be(uid);
    }
}