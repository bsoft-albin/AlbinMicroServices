using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Core.Controller
{
    public class BaseController : ControllerBase
    {

        protected IActionResult ParseApiResponse<T>(T response, HttpVerbs methodType)
        {
            if (methodType == HttpVerbs.Get)
            {
                return Ok(response);
            }
            else if (methodType == HttpVerbs.Post || methodType == HttpVerbs.Patch || methodType == HttpVerbs.Put || methodType == HttpVerbs.Delete)
            {
                if (response is ApiBaseResponse baseResponse)
                {
                    return baseResponse.StatusCode switch
                    {
                        HttpStatusCodes.Status200OK => Ok(baseResponse),
                        HttpStatusCodes.Status201Created => Created(baseResponse.StatusMessage, baseResponse),
                        HttpStatusCodes.Status202Accepted => Accepted(baseResponse.StatusMessage, baseResponse),
                        HttpStatusCodes.Status204NoContent => NoContent(),
                        HttpStatusCodes.Status301MovedPermanently => RedirectPermanent("URL"),
                        HttpStatusCodes.Status302Found => Redirect("URL"),
                        HttpStatusCodes.Status303SeeOther => RedirectToAction("ActionName"),
                        HttpStatusCodes.Status307TemporaryRedirect => RedirectPreserveMethod("URL"),
                        HttpStatusCodes.Status401Unauthorized => Unauthorized(),
                        HttpStatusCodes.Status403Forbidden => Forbid(),
                        HttpStatusCodes.Status404NotFound => NotFound(baseResponse),
                        HttpStatusCodes.Status406NotAcceptable => StatusCode(StatusCodes.Status406NotAcceptable, baseResponse),
                        HttpStatusCodes.Status415UnsupportedMediaType => new UnsupportedMediaTypeResult(),
                        HttpStatusCodes.Status409Conflict => Conflict(baseResponse),
                        HttpStatusCodes.Status422UnProcessableEntity => UnprocessableEntity(baseResponse),
                        HttpStatusCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, baseResponse),
                        _ => StatusCode(baseResponse.StatusCode, baseResponse)
                    };
                }
                else if (response is ApiErrorResponse<List<string>> errorResponse)
                {
                    return StatusCode(errorResponse.StatusCode, new
                    {
                        errorResponse.StatusCode,
                        errorResponse.StatusMessage,
                        errorResponse.ErrorDetails
                    });
                }
                else
                {
                    return StatusCode(CustomHttpStatusCodes.UnXpectedError, new { StatusMessage = CustomHttpStatusMessages.UnXpectedError, StatusCode = CustomHttpStatusCodes.UnXpectedError });
                }
            }
            else if (methodType == HttpVerbs.Options)
            {
                return Ok(new
                {
                    Methods = AllowedHttpMethods.value,
                    Description = "Allowed methods for this endpoint"
                });
            }
            else if (methodType == HttpVerbs.Headers)
            {
                return Ok("Headers Added !!");
            }
            else {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, new { StatusMessage = HttpStatusMessages.Status405MethodNotAllowed, StatusCode = HttpStatusCodes.Status405MethodNotAllowed });
            }
        }
    }
}
