using Microsoft.AspNetCore.Mvc;

namespace Schivei.Shop.Cart.Features;

/// <summary>
/// Base controller
/// </summary>
public abstract class ABaseController : ControllerBase
{
    /// <summary>
    /// Make success return
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    protected async Task<IActionResult> Ok(Task<Exception?> task)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var error = await task;

            if (error is not null)
            {
                ModelState.TryAddModelException(string.Empty, error);
                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            ModelState.TryAddModelException(string.Empty, ex);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Make success return
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    protected async Task<IActionResult> Ok<T>(Task<(T?, Exception?)> task)
    {
        try
        {
            var (data, error) = await task;

            if (error is not null)
            {
                ModelState.TryAddModelException(string.Empty, error);
                return BadRequest(ModelState);
            }

            if (data is null)
            {
                return NotFound();
            }

            return Ok(data);
        }
        catch (Exception ex)
        {
            ModelState.TryAddModelException(string.Empty, ex);
            return BadRequest(ModelState);
        }
    }
}
