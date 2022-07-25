using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

namespace Schivei.Shop.Cart.Infrastructure.Repositories;

/// <summary>
/// Checkout database and services repo
/// </summary>
public interface ICheckoutRepository
{
    /// <summary>
    /// Calculate the final payment value
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<(CheckoutResponseVM?, Exception?)> Calculate(UserId userId, CheckoutRequestVM request);

    /// <summary>
    /// Do payment and clear the cart
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="resquest"></param>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> DoPayment(UserId userId, CheckoutDoPaymentRequestVM resquest);
}
