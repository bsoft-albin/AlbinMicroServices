namespace AlbinMicroService.DataMappers.Dapper
{
    public class DapperTransactionHelper(Func<IDbConnection> connectionFactory)
    {
        public bool ExecuteTransactionQuery(string sqlQuery, object? parameters = null)
        {
            using var connection = connectionFactory();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(sqlQuery, parameters, transaction);
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                transaction.Rollback();
                return false;
            }
        }

        public bool ExecuteTransactionRawQuery(string sqlQuery)
        {
            return ExecuteTransactionQuery(sqlQuery, null);
        }

        public bool ExecuteWithSavepoint(string sqlQuery, object? parameters = null, string savepointName = "SP_1")
        {
            using IDbConnection connection = connectionFactory();
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                // Create SAVEPOINT
                transaction.Connection.Execute($"SAVEPOINT {savepointName};", transaction: transaction);

                // Execute your custom query
                transaction.Connection.Execute(sqlQuery, parameters, transaction);

                // Commit the entire transaction
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                try
                {
                    // Roll back to SAVEPOINT if possible
                    transaction.Connection.Execute($"ROLLBACK TO SAVEPOINT {savepointName};", transaction: transaction);
                    transaction.Commit(); // Optional: commit remaining part after partial rollback
                    Console.WriteLine("Rolled back to savepoint.");
                }
                catch
                {
                    // If rollback to savepoint fails, do full rollback
                    transaction.Rollback();
                    Console.WriteLine("Full rollback done.");
                }

                return false;
            }
        }
    }
}
