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
    /// <typeparam name="Y">Type of the meta data.</typeparam>
    public class ApiGenericResponse<X, Y> : ApiBaseResponse where X : new() where Y : new()
    {
        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Y MetaData { get; set; } = new();
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public X Data { get; set; } = new();
    }

    public class ApiResponse : ApiBaseResponse
    {
        public object Data { get; set; } = new { };
        public object MetaData { get; set; } = new { };
    }

    /// <summary>
    /// A Generic Class for Api Response Summary.
    /// </summary>
    public class ResponseSummary
    {
        /// <summary>
        /// Gets or sets the number of data records returned in the response.
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// Gets or sets the number of properties in the response object.
        /// </summary>
        public int PropsCount { get; set; }
    }

    /// <summary>
    /// A Generic Class for Paginated Api Response Summary.
    /// </summary>
    public class PaginatedResponseSummary
    {
        /// <summary>
        /// Gets or sets the total number of available records.
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Gets or sets the number of records per page.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }
    }
    public class PaginatedResponse : ApiBaseResponse
    {
        public object Data { get; set; } = new { };
        public PaginatedResponseSummary PaginatedSummary { get; set; } = new();
        public ResponseSummary MetaData { get; set; } = new();
    }

    public class ApiObjectResponse : ApiBaseResponse
    {
        public object Data { get; set; } = new { };
    }

    /// <summary>
    /// Standard Api error response wrapper to ensure consistent error response structure.
    /// </summary>
    /// <typeparam name="X">Type of the error data (optional).</typeparam>
    public class ApiErrorResponse<X> : ApiBaseResponse where X : new()
    {
        /// <summary>
        /// Gets or sets any additional error details.
        /// </summary>
        public X ErrorDetails { get; set; } = new();
    }
}
