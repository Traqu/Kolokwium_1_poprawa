using System.Data.SqlClient;
using Kolokwium_1.Dtos;

namespace Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IConfiguration _configuration;

        public LibraryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LibraryDto GetLibrary(int id)
        {
            var library = new LibraryDto();
            var books = new List<BookDto>();

            var connectionString = _configuration.GetConnectionString("Database");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sqlString = "SELECT Id, Name, Description, Location FROM Library WHERE Id = @id";
                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            library.Id = reader.GetInt32(0);
                            library.Name = reader.GetString(1);
                            library.Description = reader.GetString(2);
                            library.Location = reader.GetDateTime(3);
                        }
                    }
                }

                sqlString = @"                    
                    SELECT b.Id, b.Title, b.Description, b.PublicationDate, b.Rating, b.Library_Id,
                           a.Id, a.FirstName, a.LastName, a.Biography,
                           c.Id, c.Name, c.Description
                    FROM Book b
                    INNER JOIN Author a ON b.Author_Id = a.Id
                    INNER JOIN Category c ON b.Category_Id = c.Id
                    WHERE b.Library_Id = @id
                    ORDER BY b.PublicationDate DESC";
                
                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new BookDto
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                PublicationDate = reader.GetDateTime(3),
                                Rating = reader.GetInt32(4),
                                LibraryId = reader.GetInt32(5),
                                Author = new AuthorDto
                                {
                                    Id = reader.GetInt32(6),
                                    FirstName = reader.GetString(7),
                                    LastName = reader.GetString(8),
                                    Biography = reader.GetString(9)
                                },
                                Category = new CategoryDto
                                {
                                    Id = reader.GetInt32(10),
                                    Name = reader.GetString(11),
                                    Description = reader.GetString(12)
                                }
                            };
                            books.Add(book);
                        }
                    }
                }

                library.Books = books;
                connection.Close();
            }

            return library;
        }
    }
}
