using BookstoreApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BookstoreApp.Data;

public class BookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // =============================================
    // USER STORY 1 & 5: SqlDataReader (Connected - Forward-only)
    // =============================================

    public List<Book> GetAllBooks()
    {
        var books = new List<Book>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT Id, Title, Author, Price, PublicationYear, Genre FROM Books ORDER BY Id", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Price = reader.GetDecimal(3),
                PublicationYear = reader.GetInt32(4),
                Genre = reader.GetString(5)
            });
        }

        return books;
    }

    public Book? GetBookById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // USER STORY 2: Parameterized query to prevent SQL injection
        using var command = new SqlCommand("SELECT Id, Title, Author, Price, PublicationYear, Genre FROM Books WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Price = reader.GetDecimal(3),
                PublicationYear = reader.GetInt32(4),
                Genre = reader.GetString(5)
            };
        }

        return null;
    }

    // =============================================
    // USER STORY 2: Parameterized queries for Add/Update/Delete
    // =============================================

    public void AddBook(Book book)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO Books (Title, Author, Price, PublicationYear, Genre)
                    VALUES (@Title, @Author, @Price, @PublicationYear, @Genre)";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Price", book.Price);
        command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
        command.Parameters.AddWithValue("@Genre", book.Genre);

        command.ExecuteNonQuery();
    }

    public void UpdateBook(Book book)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = @"UPDATE Books
                    SET Title = @Title, Author = @Author, Price = @Price,
                        PublicationYear = @PublicationYear, Genre = @Genre
                    WHERE Id = @Id";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", book.Id);
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Price", book.Price);
        command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
        command.Parameters.AddWithValue("@Genre", book.Genre);

        command.ExecuteNonQuery();
    }

    public void DeleteBook(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("DELETE FROM Books WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }

    // =============================================
    // USER STORY 3: Stored Procedures
    // =============================================

    public List<Book> GetAllBooksViaStoredProcedure()
    {
        var books = new List<Book>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_GetAllBooks", connection);
        command.CommandType = CommandType.StoredProcedure;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Price = reader.GetDecimal(3),
                PublicationYear = reader.GetInt32(4),
                Genre = reader.GetString(5)
            });
        }

        return books;
    }

    public Book? GetBookByIdViaStoredProcedure(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_GetBookById", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Price = reader.GetDecimal(3),
                PublicationYear = reader.GetInt32(4),
                Genre = reader.GetString(5)
            };
        }

        return null;
    }

    public int AddBookViaStoredProcedure(Book book)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_AddBook", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Price", book.Price);
        command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
        command.Parameters.AddWithValue("@Genre", book.Genre);

        var result = command.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    public void UpdateBookViaStoredProcedure(Book book)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_UpdateBook", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", book.Id);
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Price", book.Price);
        command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
        command.Parameters.AddWithValue("@Genre", book.Genre);

        command.ExecuteNonQuery();
    }

    public void DeleteBookViaStoredProcedure(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_DeleteBook", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }

    // =============================================
    // USER STORY 4 & 5: DataSet / DataTable (Disconnected Architecture)
    // =============================================

    public DataSet GetAllBooksAsDataSet()
    {
        using var connection = new SqlConnection(_connectionString);

        var adapter = new SqlDataAdapter("SELECT Id, Title, Author, Price, PublicationYear, Genre FROM Books", connection);

        var dataSet = new DataSet();
        adapter.Fill(dataSet, "Books");

        return dataSet;
    }

    public List<Book> GetAllBooksFromDataSet()
    {
        var dataSet = GetAllBooksAsDataSet();
        var table = dataSet.Tables["Books"]!;

        var books = new List<Book>();

        foreach (DataRow row in table.Rows)
        {
            books.Add(new Book
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = row["Title"].ToString()!,
                Author = row["Author"].ToString()!,
                Price = Convert.ToDecimal(row["Price"]),
                PublicationYear = Convert.ToInt32(row["PublicationYear"]),
                Genre = row["Genre"].ToString()!
            });
        }

        return books;
    }

    public void UpdateDatabaseFromDataSet(DataSet dataSet)
    {
        using var connection = new SqlConnection(_connectionString);

        var adapter = new SqlDataAdapter("SELECT Id, Title, Author, Price, PublicationYear, Genre FROM Books", connection);

        var builder = new SqlCommandBuilder(adapter);
        adapter.Update(dataSet, "Books");
    }

    public DataSet GetBooksDataSetWithChanges()
    {
        var dataSet = GetAllBooksAsDataSet();
        var table = dataSet.Tables["Books"]!;

        // Demonstrate CRUD on DataTable (disconnected)
        // Add a new row
        var newRow = table.NewRow();
        newRow["Title"] = "Sample Book";
        newRow["Author"] = "Sample Author";
        newRow["Price"] = 29.99m;
        newRow["PublicationYear"] = 2024;
        newRow["Genre"] = "Fiction";
        table.Rows.Add(newRow);

        // Update an existing row (if any)
        if (table.Rows.Count > 1)
        {
            table.Rows[0]["Price"] = 19.99m;
        }

        // Delete a row (if more than 2 exist)
        if (table.Rows.Count > 2)
        {
            table.Rows[1].Delete();
        }

        return dataSet;
    }
}