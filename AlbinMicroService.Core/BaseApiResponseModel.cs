/*
 * ----------------------------------------------------------------------------
 *  File:         BaseApiResponseModel.cs
 *  Project:      AlbinMicroService
 *  Namespace:    AlbinMicroService.Core
 *  Author:       [Albin_Anthony]
 *  Created On:   [31-01-2025]
 *  Modified By:  [Albin_Anthony] on [31-01-2025]
 *  Description:  A Generic Api response wrapper to maintain consistent Api 
 *                response structure across all services.
 * ----------------------------------------------------------------------------
 */

namespace AlbinMicroService.Core
{
    /// <summary>
    /// Standard Api response wrapper to ensure consistent Api response structure.
    /// </summary>
    /// <typeparam name="X">Type of the response data.</typeparam>
    public class ApiResponse<X> where X : class, new()
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        public short StatusCode { get; set; } = 200;
        /// <summary>
        /// Gets or sets the status message of the response.
        /// </summary>
        public string StatusMessage { get; set; } = "OK";
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public X Data { get; set; } = new X();
    }

    /// <summary>
    /// Standard Api error response wrapper to ensure consistent error response structure.
    /// </summary>
    /// <typeparam name="X">Type of the error data (optional).</typeparam>
    public class ApiErrorResponse<X> where X : class, new()
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the error response.
        /// </summary>
        public short StatusCode { get; set; } = 500;

        /// <summary>
        /// Gets or sets the error message describing what went wrong.
        /// </summary>
        public string ErrorMessage { get; set; } = "An unexpected error occurred.";

        /// <summary>
        /// Gets or sets the error code (useful for client-side handling).
        /// </summary>
        public string ErrorCode { get; set; } = "UNKNOWN_ERROR";

        /// <summary>
        /// Gets or sets any additional error details.
        /// </summary>
        public X ErrorDetails { get; set; } = new X();
    }
}
