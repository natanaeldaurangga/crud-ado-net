using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Entities;
using System.Data.SqlClient;
using System.Data;
using BookApi.DTOs;

namespace BookApi.Repositories
{
    public class BookRepository
    {
        private readonly IConfiguration _config;
        private readonly string ConnectionString;

        public BookRepository(IConfiguration config)
        {
            _config = config;
            this.ConnectionString = _config.GetConnectionString("DefaultConnection")!;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            const string commandText = "SELECT * FROM Books";

            var books = new List<Book>();

            using (var command = new SqlCommand(commandText, (SqlConnection)connection))
            {
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var book = new Book
                        {
                            Id = Guid.Parse(reader.GetString(reader.GetOrdinal("ID")) ?? string.Empty),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("BookDescription")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            Created = reader.GetDateTime(reader.GetOrdinal("Created")),
                            Updated = reader.GetDateTime(reader.GetOrdinal("Updated")),
                        };

                        books.Add(book);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }

            }
            return books;
        }

        public bool InsertBook(BookDTO dto)
        {
            bool result = false;
            var bookId = Guid.NewGuid().ToString();
            var created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var commandText = "INSERT INTO Books(ID, Title, BookDescription, Author, Stock, Created, Updated)" +
            $"\nVALUES (@ID, @Title, @BookDescription, @Author, @Stock, @Created, @Updated)";
            using (var connection = new SqlConnection(ConnectionString))
            using (SqlCommand com = new())
            {
                com.Connection = (SqlConnection)connection;
                com.CommandText = commandText;
                com.Parameters.AddWithValue("@ID", bookId);
                com.Parameters.AddWithValue("@Title", dto.Title);
                com.Parameters.AddWithValue("@BookDescription", dto.Description);
                com.Parameters.AddWithValue("@Author", dto.Author);
                com.Parameters.AddWithValue("@Stock", dto.Stock);
                com.Parameters.AddWithValue("@Created", created);
                com.Parameters.AddWithValue("@Updated", updated);

                try
                {
                    connection.Open();

                    result = com.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally { connection.Close(); }
            }
            return result;
        }

        public bool UpdateBook(string id, BookDTO dto)
        {
            bool result = false;
            var updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var commandText = "UPDATE Books\n"
            + $"SET Title = @Title, BookDescription = @Description, Author = @Author, Stock = @Stock, Updated = @Updated\n"
            + $"WHERE ID = '{id}'";
            using (var connection = new SqlConnection(ConnectionString))
            using (SqlCommand com = new())
            {
                com.Connection = (SqlConnection)connection;
                com.CommandText = commandText;
                com.Parameters.AddWithValue("@Title", dto.Title);
                com.Parameters.AddWithValue("@BookDescription", dto.Description);
                com.Parameters.AddWithValue("@Author", dto.Author);
                com.Parameters.AddWithValue("@Stock", dto.Stock);
                com.Parameters.AddWithValue("@Updated", updated);

                try
                {
                    connection.Open();

                    result = com.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally { connection.Close(); }
            }
            return result;
        }


        // TODO: Lanjut delete
        public bool DeleteBook(string id)
        {
            bool result = false;
            var commandText = $"DELETE FROM Books WHERE ID = '{id}'";
            using (var connection = new SqlConnection(ConnectionString))
            using (SqlCommand com = new())
            {
                com.Connection = (SqlConnection)connection;
                com.CommandText = commandText;

                try
                {
                    connection.Open();

                    result = com.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally { connection.Close(); }
            }
            return result;
        }

    }
}