using AlbinMicroService.Core.Utilities;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Core.Controller
{
    public class BaseController : ControllerBase
    {

        protected IActionResult ParseApiResponse<T>(T response, HttpVerbs methodType, bool isPaginated = false)
        {
            if (response == null)
            {
                return NoContent();
            }
            else
            {
                if (methodType == HttpVerbs.Get)
                {
                    if (response is ApiResponse data)
                    {
                        if (data.Data != null && data.Data is IEnumerable<object> newList)
                        {
                            if (newList != null && newList.Any())
                            {
                                data.MetaData = new ResponseSummary { DataCount = newList.Count(), PropsCount = StaticMeths.GetPropertyCount(newList.First()) };
                            }
                        }

                        return Ok(data);
                    }
                    else if (isPaginated && response is PaginatedResponse paginatedData)
                    {
                        // further processing on Paging Response , do it below ...
                        if (paginatedData.Data != null && paginatedData.Data is IEnumerable<object> pageList)
                        {
                            if (pageList != null && pageList.Any())
                            {
                                paginatedData.MetaData = new ResponseSummary { DataCount = pageList.Count(), PropsCount = StaticMeths.GetPropertyCount(pageList.First()) };
                            }
                        }

                        return Ok(paginatedData);
                    }
                    else if (response is ApiGenericResponse<object, ResponseSummary> singleObject)
                    {
                        singleObject.MetaData.DataCount++;
                        if (singleObject.Data != null)
                        {
                            singleObject.MetaData.PropsCount = StaticMeths.GetPropertyCount(singleObject.Data);
                        }

                        return Ok(singleObject);
                    }
                    else if (response is ApiObjectResponse apiObjectResponse)
                    {
                        return Ok(apiObjectResponse);
                    }
                    else
                    {
                        return Ok(response);
                    }
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
                            HttpStatusCodes.Status206PartialContent => StatusCode(StatusCodes.Status206PartialContent, baseResponse),
                            HttpStatusCodes.Status301MovedPermanently => RedirectPermanent("URL"),
                            HttpStatusCodes.Status302Found => Redirect("URL"),
                            HttpStatusCodes.Status303SeeOther => RedirectToAction("ActionName"),
                            HttpStatusCodes.Status307TemporaryRedirect => RedirectPreserveMethod("URL"),
                            HttpStatusCodes.Status401Unauthorized => Unauthorized(),
                            HttpStatusCodes.Status403Forbidden => Forbid(),
                            HttpStatusCodes.Status404NotFound => NotFound(baseResponse),
                            HttpStatusCodes.Status406NotAcceptable => StatusCode(StatusCodes.Status406NotAcceptable, baseResponse),
                            HttpStatusCodes.Status408RequestTimeout => StatusCode(StatusCodes.Status408RequestTimeout, baseResponse),
                            HttpStatusCodes.Status410Gone => StatusCode(StatusCodes.Status410Gone, baseResponse),
                            HttpStatusCodes.Status412PreconditionFailed => StatusCode(StatusCodes.Status412PreconditionFailed, baseResponse),
                            HttpStatusCodes.Status415UnsupportedMediaType => new UnsupportedMediaTypeResult(),
                            HttpStatusCodes.Status409Conflict => Conflict(baseResponse),
                            HttpStatusCodes.Status422UnProcessableEntity => UnprocessableEntity(baseResponse),
                            HttpStatusCodes.Status426UpgradeRequired => StatusCode(StatusCodes.Status426UpgradeRequired, baseResponse),
                            HttpStatusCodes.Status429TooManyRequests => StatusCode(StatusCodes.Status429TooManyRequests, baseResponse),
                            HttpStatusCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, baseResponse),
                            HttpStatusCodes.Status501NotImplemented => StatusCode(StatusCodes.Status501NotImplemented, baseResponse),
                            HttpStatusCodes.Status502BadGateway => StatusCode(StatusCodes.Status502BadGateway, baseResponse),
                            HttpStatusCodes.Status503ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, baseResponse),
                            HttpStatusCodes.Status504GatewayTimeout => StatusCode(StatusCodes.Status504GatewayTimeout, baseResponse),
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
                    //Response.Headers.Add("efw");
                    return Ok("Headers Added !!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, new { StatusMessage = HttpStatusMessages.Status405MethodNotAllowed, StatusCode = HttpStatusCodes.Status405MethodNotAllowed });
                }
            }

        }

        protected IActionResult MakeStatusCodeResponse(ApiBaseResponse response)
        {
            if (response != null && response is ApiBaseResponse baseResponse)
            {
                return baseResponse.StatusCode switch
                {
                    HttpStatusCodes.Status200OK => StatusCode(StatusCodes.Status200OK, baseResponse),
                    HttpStatusCodes.Status201Created => Created(baseResponse.StatusMessage, baseResponse),
                    HttpStatusCodes.Status202Accepted => Accepted(baseResponse.StatusMessage, baseResponse),
                    HttpStatusCodes.Status204NoContent => NoContent(),
                    HttpStatusCodes.Status206PartialContent => StatusCode(StatusCodes.Status206PartialContent, baseResponse),
                    HttpStatusCodes.Status301MovedPermanently => RedirectPermanent("URL"),
                    HttpStatusCodes.Status302Found => Redirect("URL"),
                    HttpStatusCodes.Status303SeeOther => RedirectToAction("ActionName"),
                    HttpStatusCodes.Status307TemporaryRedirect => RedirectPreserveMethod("URL"),
                    HttpStatusCodes.Status401Unauthorized => Unauthorized(),
                    HttpStatusCodes.Status403Forbidden => Forbid(),
                    HttpStatusCodes.Status404NotFound => NotFound(baseResponse),
                    HttpStatusCodes.Status406NotAcceptable => StatusCode(StatusCodes.Status406NotAcceptable, baseResponse),
                    HttpStatusCodes.Status408RequestTimeout => StatusCode(StatusCodes.Status408RequestTimeout, baseResponse),
                    HttpStatusCodes.Status410Gone => StatusCode(StatusCodes.Status410Gone, baseResponse),
                    HttpStatusCodes.Status412PreconditionFailed => StatusCode(StatusCodes.Status412PreconditionFailed, baseResponse),
                    HttpStatusCodes.Status415UnsupportedMediaType => new UnsupportedMediaTypeResult(),
                    HttpStatusCodes.Status409Conflict => Conflict(baseResponse),
                    HttpStatusCodes.Status422UnProcessableEntity => UnprocessableEntity(baseResponse),
                    HttpStatusCodes.Status426UpgradeRequired => StatusCode(StatusCodes.Status426UpgradeRequired, baseResponse),
                    HttpStatusCodes.Status429TooManyRequests => StatusCode(StatusCodes.Status429TooManyRequests, baseResponse),
                    HttpStatusCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, baseResponse),
                    HttpStatusCodes.Status501NotImplemented => StatusCode(StatusCodes.Status501NotImplemented, baseResponse),
                    HttpStatusCodes.Status502BadGateway => StatusCode(StatusCodes.Status502BadGateway, baseResponse),
                    HttpStatusCodes.Status503ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, baseResponse),
                    HttpStatusCodes.Status504GatewayTimeout => StatusCode(StatusCodes.Status504GatewayTimeout, baseResponse),
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
        protected IActionResult MakeOkObjectResponse(ApiResponse response)
        {
            if (response != null)
            {
                if (response is ApiResponse data)
                {
                    if (data.Data != null && data.Data is IEnumerable<object> newList)
                    {
                        if (newList != null && newList.Any())
                        {
                            data.MetaData = new ResponseSummary { DataCount = newList.Count(), PropsCount = StaticMeths.GetPropertyCount(newList.First()) };
                        }
                    }

                    return Ok(data);
                }
                //else if (response is ApiResponse<List<object>, PaginatedResponseSummary> pagintedData)
                //{
                //    // this needs to be changed, as per Paging
                //    if (pagintedData.Data is List<object> newList)
                //    {
                //        pagintedData.MetaData.TotalRecords = newList.Count;
                //        pagintedData.MetaData.PageSize = 25;
                //        pagintedData.MetaData.CurrentPage = 1; // means first page
                //    }

                //    return Ok(pagintedData);
                //}
                //else if (response is ApiResponse<object, ResponseSummary> singleObject)
                //{
                //    singleObject.Data = response;
                //    singleObject.MetaData.DataCount++;
                //    if (singleObject.Data != null)
                //    {
                //        singleObject.MetaData.PropsCount = StaticMeths.GetPropertyCount(singleObject.Data);
                //    }

                //    // here need to check all possible scenarios.
                //    return Ok(response);
                //}
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return NoContent();
            }
        }

        protected IActionResult GetResponseHeaders()
        {
            // here in this response, make possible of sending all headers values of the Response.
            Response.Headers.Add("X-Custom-Header", "CustomHeaderValue");
            return Ok(Response.Headers);
        }

        protected IActionResult GetOptionMethods()
        {
            return Ok(new
            {
                Methods = AllowedHttpMethods.value,
                Description = "Allowed methods for this endpoint"
            });
        }
    }
}
