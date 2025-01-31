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
            { 503, "Record Reffered" },
            { 422, "UnProcessable Entity" },
            { 204, "Entity Not Found" }
        };


        // Created By Albin_Anthony for Generic Api Response Structure.
        protected IActionResult ParseApiResponse<T>(T input, HttpVerbs methodType = HttpVerbs.Get)
        {
            // Create an instance of ApiResponse
            ApiResponse<object> apiResponse = new();

            // Check for null input
            if (input == null)
            {
                return NoContent();
            }

            try
            {
                if (methodType == HttpVerbs.Get)
                {
                    // Check if input is an IEnumerable
                    if (input is IEnumerable<object> enumerable)
                    {
                        List<object> list = enumerable.ToList(); // Convert to List<T> for processing
                        apiResponse.Data = list;
                        //apiResponse.MetaData.Count = list.Count;

                        if (list.Count > 0)
                        {
                            //apiResponse.MetaData.PropsCount = CommonMethods.GetPropertyCount(list[0]); // Use the first item
                        }
                    }
                    else // Single object case
                    {
                        apiResponse.Data = input;
                        //apiResponse.MetaData.Count = 1; // Single object means count is 1
                        //apiResponse.MetaData.PropsCount = CommonMethods.GetPropertyCount(input);
                    }
                }
                else
                {
                    short DbResponseCode = Convert.ToInt16(input);

                    if (DbResponseCode == 205 || DbResponseCode == 202 || DbResponseCode == 201)
                    {
                        if (methodType == HttpVerbs.Post)
                        {
                            return StatusCode(StatusCodes.Status201Created, "Inserted Successfully");
                        }
                        else if (methodType == HttpVerbs.Put)
                        {
                            return StatusCode(StatusCodes.Status202Accepted, "Updated Successfully");
                        }
                        else if (methodType == HttpVerbs.Delete)
                        {
                            return StatusCode(StatusCodes.Status200OK, "Deleted Successfully");
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status405MethodNotAllowed, "Method not Allowed !!");
                        }
                    }
                    else
                    {
                        // Check if the key exists in the dictionary
                        if (kvpResponseCode.TryGetValue(DbResponseCode, out string value))
                        {
                            apiResponse.StatusCode = DbResponseCode;
                            apiResponse.StatusMessage = value;
                            return StatusCode(DbResponseCode, value); // Return the value if the key exists
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status405MethodNotAllowed, "Method not Allowed !!");
                        }
                    }

                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return StatusCode(StatusCodes.Status500InternalServerError, input);
            }

            return Ok(apiResponse);
        }

    }
}
