using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Core.Controller
{
    public class BaseController : ControllerBase
    {
        // Define key-value pairs
        private static Dictionary<int, string> kvpResponseCode = new()
        {
            { 501, "Record Already Exists" },
            { 502, "Can't delete" },
            { 503, "Record Referred" },
            { 422, "UnProcessable Entity" },
            { 204, "Entity Not Found" }
        };


        // Created By Albin_Anthony for Generic Api Response Structure.
        protected IActionResult ParseApiResponse<T>(T input, HttpVerbs methodType = HttpVerbs.Get)
        {
            return Ok(input);
        }

    }
}
