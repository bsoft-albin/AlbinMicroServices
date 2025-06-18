using System.Data.Common;
using Npgsql;

namespace AlbinMicroService.Core.Utilities
{
    public static class ExceptionExtensions
    {
        public static ErrorObject ToErrorObject(this Exception ex)
        {
            return ex switch
            {
                //UniqueConstraintException => new ErrorObject
                //{
                //    Code = "DUPLICATE_KEY",
                //    Message = "A record with the same key already exists."
                //},
                //CannotInsertNullException => new ErrorObject
                //{
                //    Code = "NULL_VALUE",
                //    Message = "A required field was null."
                //},
                //DbUpdateException => new ErrorObject
                //{
                //    Code = "UPDATE_FAILURE",
                //    Message = "Could not save changes to the database."
                //},
                //MySqlException mySqlEx when mySqlEx.Number == 1045 => new ErrorObject
                //{
                //    Code = "MYSQL_AUTH_ERROR",
                //    Message = "Access denied for user (authentication failed)."
                //},
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
    }
}
