using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;

namespace Schivei.Shop.Cart.Features;

/// <summary>
/// Checkout Endpoint
/// </summary>
[ApiController]
[Route("[controller]")]
public class CheckoutController : ABaseController
{
    private readonly ICheckoutRepository _checkoutRepository;

    /// <inheritdoc/>
    public CheckoutController(ICheckoutRepository checkoutRepository) => _checkoutRepository = checkoutRepository;

    /// <summary>
    /// Calculate the final payment value
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{userId:Guid}")]
    [ProducesDefaultResponseType(typeof(CheckoutResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> Calculate([FromRoute] Guid userId, [FromBody] CheckoutRequestVM request) =>
        Ok(_checkoutRepository.Calculate(userId, request));

    /// <summary>
    /// Do payment and clear the cart
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="requestVM"></param>
    /// <returns></returns>
    [HttpPut("{userId:Guid}")]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> DoPayment([FromRoute] Guid userId, [FromBody] CheckoutDoPaymentRequestVM requestVM) =>
        Ok(_checkoutRepository.DoPayment(userId, requestVM));
}
