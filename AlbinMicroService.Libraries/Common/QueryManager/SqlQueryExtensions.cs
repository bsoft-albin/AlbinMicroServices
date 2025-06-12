namespace AlbinMicroService.Libraries.Common.QueryManager
{
    public static class SqlQueryExtensions
    {
        /// <summary>
        /// Extension method to get query from current assembly
        /// </summary>
        public static string GetSqlQuery(this object instance, string fileName, string queryName)
        {
            return SqlQueryManager.GetQuery(fileName, queryName, instance.GetType().Assembly);
        }
    }
}
