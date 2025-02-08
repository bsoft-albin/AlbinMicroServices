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

using AlbinMicroService.Core.Utilities;
using static AlbinMicroService.Core.Utilities.StaticProps;

namespace AlbinMicroService.Core
{
    public class ApiBaseResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        public short StatusCode { get; set; } = HttpStatusCodes.Status200OK;
        /// <summary>
        /// Gets or sets the status message of the response.
        /// </summary>
        public string StatusMessage { get; set; } = HttpStatusMessages.Status200OK;
    }

    /// <summary>
    /// Standard Api response wrapper to ensure consistent Api response structure.
    /// </summary>
    /// <typeparam name="X">Type of the response data.</typeparam>
    public class ApiResponse<X> where X : ApiBaseResponse, new()
    {
        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public ResponseSummary MetaData { get; set; } = new();
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public X Data { get; set; } = new X();
    }

    public class ResponseSummary
    {
        public int DataCount { get; set; }
        public short PropsCount { get; set; }
    }

    public class PaginatedResponse
    {
        public int TotalRecords { get; set; } // Total available records (optional, for paginated responses)
        public int PageSize { get; set; } // Number of records per page (optional, for paginated responses)
        public int CurrentPage { get; set; } // Current page number (optional, for paginated responses)
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
        public short StatusCode { get; set; } = HttpStatusCodes.Status500InternalServerError;

        /// <summary>
        /// Gets or sets the error message describing what went wrong.
        /// </summary>
        public string ErrorMessage { get; set; } = CustomHttpStatusMessages.UnXpectedError;

        /// <summary>
        /// Gets or sets the error code (useful for client-side handling).
        /// </summary>
        public string ErrorCode { get; set; } = CustomHttpStatusMessages.UnKnownError;

        /// <summary>
        /// Gets or sets any additional error details.
        /// </summary>
        public X ErrorDetails { get; set; } = new X();
    }
}
