using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperHero.Service.Infra.Exceptions;
using System.Net;

namespace SuperHero.Service.Infra.Web;

public abstract class ManagedController : ControllerBase
{
    private readonly ILogger<ManagedController> _logger;

    protected ManagedController(ILogger<ManagedController> logger)
    {
        _logger = logger;
    }

    protected async Task<IActionResult> TryExecuteOK(Func<Task<object>> pExecute)
    {
        Func<object, IActionResult> action = result => Ok(result);
        return await TryExecute(action, pExecute);
    }

    protected async Task<IActionResult> TryExecuteDelete(Func<Task<object>> pExecute)
    {
        Func<object, IActionResult> action = result =>
        {
            bool success = (bool)result;
            return new ObjectResult(null) { StatusCode = success ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.NotFound };
        };
        return await TryExecute(action, pExecute);
    }

    protected async Task<IActionResult> TryExecute(Func<object, IActionResult> pResultFunc, Func<Task<object>> pExecute)
    {
        try
        {
            object result = await pExecute();
            return pResultFunc(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ForbidException ex)
        {
            return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.Forbidden };
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DomainRuleException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DuplicateRegistrationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in running the Api.");
            return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}