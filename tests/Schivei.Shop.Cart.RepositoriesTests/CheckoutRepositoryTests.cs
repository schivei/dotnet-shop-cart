using FluentAssertions;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Models;
using Xunit;

namespace Schivei.Shop.Cart.Repositories.Tests;

public class CheckoutRepositoryTests
{
    private readonly CheckoutRepository _repository;
    private readonly CartRepository _cartRepository;

    public CheckoutRepositoryTests() =>
        (_cartRepository, _repository) = (new(), new());

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
    public async Task CalculateTest(string userId)
    {
        var product = DataMocking.SKUs.First().Value;

        UserId uid = new Guid(userId);

        var requestCart = new CreateCartWithItemsRequestVM
        {
            Quantity = 3,
            Sku = product.Sku,
            UserId = uid
        };

        var response = await _cartRepository.Create(requestCart);

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        var cart = response.Item1!.Value;

        cart.Items.Should().HaveCount(1);
        cart.Items.First().Quantity.Should().Be(3);

        var request = new CheckoutRequestVM
        {
            ZipCode = "13530000",
            Coupon = "OFF15"
        };

        var respc = await _repository.Calculate(uid, request);

        respc.Item1.Should().NotBeNull();
        respc.Item2.Should().BeNull();

        var checkout = respc.Item1!.Value;

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
    public async Task DoPaymentTest(string userId)
    {
        await CalculateTest(userId);

        var request = new CheckoutDoPaymentRequestVM
        {
            CardName = "TEST CARD",
            CardNumber = "1234567890123456",
            CvcCode = "123"
        };

        UserId uid = new Guid(userId);

        var response = await _repository.DoPayment(uid, request);

        response.Item1.Should().NotBeNull();
        response.Item2.Should().BeNull();

        var cart = response.Item1!.Value;

        cart.UserId.Should().Be(new Guid(userId));
        cart.Items.Should().BeEmpty();
    }
}
