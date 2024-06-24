using System.Data.SqlClient;
using System.Transactions;

namespace Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IConfiguration _configuration;

        public AuthorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void DeleteAuthor(int id)
        {
            var connectionString = _configuration.GetConnectionString("Database");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var sqlString = "DELETE FROM Book WHERE Author_Id = @id";
                        
                        using (var command = new SqlCommand(sqlString, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        sqlString = "DELETE FROM Author WHERE Id = @id";
                        
                        using (var command = new SqlCommand(sqlString, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw new TransactionException();
                    }
                }
                connection.Close();
            }
        }
    }
}