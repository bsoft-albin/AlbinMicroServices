using AlbinMicroService.Core;
using AlbinMicroService.Core.Utilities;
using Npgsql;

namespace AlbinMicroService.DataMappers.Utilities
{
    public static class DataMapperExtensions
    {
        public static ErrorObject ToErrorObject(this Exception ex)
        {
            return ex switch
            {
                MySqlException mySqlEx when mySqlEx.Number == 1045 => new ErrorObject
                {
                    Code = "MYSQL_AUTH_ERROR",
                    Message = "Access denied for user (authentication failed)."
                },
                DbUpdateException => new ErrorObject
                {
                    Code = "UPDATE_FAILURE",
                    Message = "Could not save changes to the database."
                },
                PostgresException pgEx => new ErrorObject
                {
                    Code = "POSTGRESQL_ERROR",
                    Message = pgEx.Message
                },
                DbException => new ErrorObject
                {
                    Code = "DATABASE_ERROR",
                    Message = ex.Message
                },
                InvalidOperationException => new ErrorObject
                {
                    Code = "MAPPING_ERROR",
                    Message = "An unexpected result or operation occurred"
                },
                ArgumentException => new ErrorObject
                {
                    Code = "BAD_ARGUMENT",
                    Message = "One or more arguments are invalid"
                },
                NullReferenceException => new ErrorObject
                {
                    Code = "NULL_REFERENCE",
                    Message = "Object reference not set to an instance of an object"
                },
                HttpRequestException => new ErrorObject
                {
                    Code = "HTTP_ERROR",
                    Message = "An error occurred while making an HTTP request"
                },
                TaskCanceledException => new ErrorObject
                {
                    Code = "DATABASE_TIMEOUT",
                    Message = "The operation timed out"
                },
                _ => new ErrorObject
                {
                    Code = "UNKNOWN_ERROR",
                    Message = ex.Message
                }
            };
        }

        public static GenericObjectSwitcher<DataType> DoExceptionFlow<DataType>(this GenericObjectSwitcher<DataType> genericObject, Exception exception) where DataType : new()
        {
            genericObject.Error = exception.Message;
            genericObject.IsErrorHappened = Literals.Boolean.True;
            genericObject.ErrorData = exception.ToErrorObject();

            return genericObject;
        }

        public static GenericObjectSwitcherNull<DataType> DoExceptionFlow<DataType>(this GenericObjectSwitcherNull<DataType> genericObject, Exception exception) where DataType : class
        {
            genericObject.Error = exception.Message;
            genericObject.IsErrorHappened = Literals.Boolean.True;
            genericObject.ErrorData = exception.ToErrorObject();

            return genericObject;
        }

        public static async Task<PaginatedResponse> ToPaginatedResponseAsync<DataSource>(this IQueryable<DataSource> source, int page, int pageSize, bool includeMetaData = false, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            // Deferred execution — fetch paginated data first
            int skip = (page - 1) * pageSize;
            IQueryable<DataSource> query = source.Skip(skip).Take(pageSize);

            // Execute query for current page only
            Task<List<DataSource>> itemsTask = query.ToListAsync(cancellationToken);

            // Count only if needed (you can remove this if your frontend doesn’t use total)
            Task<int> countTask = source.CountAsync(cancellationToken);

            await Task.WhenAll(itemsTask, countTask);

            List<DataSource> items = itemsTask.Result;
            int totalRecords = countTask.Result;

            // Optional: Avoid reflection-based metadata for large T or big record counts
            int propsCount = includeMetaData ? typeof(DataSource).GetProperties().Length : 0;

            return new PaginatedResponse
            {
                Data = items,
                PaginatedSummary = new PaginatedResponseSummary
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                },
                MetaData = new ResponseSummary
                {
                    DataCount = items.Count,
                    PropsCount = propsCount
                }
            };
        }
    }
}
