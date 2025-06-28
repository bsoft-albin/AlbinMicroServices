namespace AlbinMicroService.DataMappers.RawADO
{
    public class DbTransactionHelper(DbProviderFactory factory, string connectionString)
    {
        public bool ExecuteTransactionQuery(string sqlQuery, List<DbParameter>? parameters = null)
        {
            using DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using var transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = sqlQuery;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            try
            {
                command.ExecuteNonQuery();
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

        // Savepoint
        public bool ExecuteWithSavepoint(string sqlQuery, List<DbParameter>? parameters = null, string savepointName = "SP_1")
        {
            using var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                // Create the SAVEPOINT
                using (var savepointCmd = connection.CreateCommand())
                {
                    savepointCmd.Connection = connection;
                    savepointCmd.Transaction = transaction;
                    savepointCmd.CommandText = $"SAVEPOINT {savepointName};";
                    savepointCmd.ExecuteNonQuery();
                }

                // Execute the user query
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = sqlQuery;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                            command.Parameters.Add(param);
                    }

                    command.ExecuteNonQuery();
                }

                // Commit the transaction
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");

                try
                {
                    // Rollback to savepoint
                    using (var rollbackCmd = connection.CreateCommand())
                    {
                        rollbackCmd.Connection = connection;
                        rollbackCmd.Transaction = transaction;
                        rollbackCmd.CommandText = $"ROLLBACK TO SAVEPOINT {savepointName};";
                        rollbackCmd.ExecuteNonQuery();
                    }

                    // Commit any remaining transaction (optional, or could rollback completely)
                    transaction.Commit();
                    Console.WriteLine("Rolled back to savepoint and committed remaining.");
                }
                catch (Exception rollbackEx)
                {
                    Console.WriteLine($"Rollback to savepoint failed: {rollbackEx.Message}");
                    transaction.Rollback();
                    Console.WriteLine("Full rollback done.");
                }

                return false;
            }
        }
    }
}
