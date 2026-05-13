# 📚 Bookstore Management Application

A full-featured **ASP.NET Core MVC** web application for managing a bookstore's inventory, built with **ADO.NET** for direct database access. This project demonstrates enterprise-grade data access patterns including connected and disconnected architectures, stored procedures, and SQL injection prevention.

---

## 🚀 Features

| Feature | Description |
|---------|-------------|
| **CRUD Operations** | Create, Read, Update, and Delete books using `SqlConnection` and `SqlCommand` |
| **SQL Injection Prevention** | All queries use parameterized commands (`@Parameter` syntax) |
| **Stored Procedures** | Full CRUD via stored procedures (`sp_GetAllBooks`, `sp_GetBookById`, `sp_AddBook`, `sp_UpdateBook`, `sp_DeleteBook`) |
| **Disconnected Architecture** | `DataSet` / `DataTable` with `SqlDataAdapter` for offline data manipulation |
| **Connected Architecture** | `SqlDataReader` for fast, forward-only data reading |
| **Bootstrap 5 UI** | Clean, responsive interface with modern styling |

---

## 🏗️ Architecture

```
BookstoreApp/
├── Controllers/
│   ├── BookController.cs        # CRUD + SP + DataSet endpoints
│   └── HomeController.cs        # Landing page
├── Data/
│   └── BookRepository.cs        # ADO.NET data access layer
├── Models/
│   └── Book.cs                  # Book entity model
├── Views/
│   ├── Book/
│   │   ├── Index.cshtml         # Book listing table
│   │   ├── Details.cshtml       # Single book view
│   │   ├── Create.cshtml        # Add new book form
│   │   ├── Edit.cshtml          # Edit book form
│   │   └── Delete.cshtml        # Delete confirmation
│   ├── Home/
│   │   └── Index.cshtml         # Welcome page
│   └── Shared/
│       ├── _Layout.cshtml       # Bootstrap 5 layout
│       └── _ValidationScriptsPartial.cshtml
├── Scripts/
│   └── DatabaseSetup.sql        # DB schema + stored procedures
├── Program.cs                   # App startup & DI configuration
├── appsettings.json             # Connection string
└── BookstoreApp.csproj          # .NET 10 project file
```

---

## 📋 User Stories Covered

### User Story 1: CRUD with SqlConnection & SqlCommand
- **Index** — View all books in a table
- **Details** — View a single book's information
- **Create** — Add a new book with form validation
- **Edit** — Modify existing book details
- **Delete** — Remove a book with confirmation

### User Story 2: SQL Injection Prevention
All database operations use **parameterized queries**:
```csharp
command.Parameters.AddWithValue("@Title", book.Title);
command.Parameters.AddWithValue("@Author", book.Author);
```

### User Story 3: Stored Procedures
Five stored procedures handle all CRUD operations:
- `sp_GetAllBooks` — Retrieve all books
- `sp_GetBookById` — Retrieve a single book by ID
- `sp_AddBook` — Insert a new book
- `sp_UpdateBook` — Update an existing book
- `sp_DeleteBook` — Delete a book by ID

Accessible via `/Book/IndexSP`, `/Book/CreateSP`, `/Book/EditSP/{id}`, `/Book/DeleteSP/{id}`

### User Story 4: DataSet & DataTable (Disconnected)
- `GetAllBooksAsDataSet()` — Fills a `DataSet` using `SqlDataAdapter`
- `UpdateDatabaseFromDataSet()` — Pushes `DataSet` changes back to the database
- Accessible via `/Book/IndexDataSet`

### User Story 5: SqlDataReader & SqlDataAdapter
- **SqlDataReader** — Used in `GetAllBooks()` and `GetBookById()` for fast, forward-only reads
- **SqlDataAdapter** — Used in `GetAllBooksAsDataSet()` for disconnected data access

---

## 🔧 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any SQL Server edition)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional)

---

## ⚙️ Setup & Installation

### 1. Clone the Repository
```bash
git clone https://github.com/DibyaGit/ADO.Net-EF-Core-Bookstore-Management-App.git
cd ADO.Net-EF-Core-Bookstore-Management-App/BookstoreApp
```

### 2. Set Up the Database
Run the SQL setup script using **sqlcmd** or **SSMS**:

**Using sqlcmd:**
```bash
sqlcmd -S localhost\SQLEXPRESS -E -i Scripts/DatabaseSetup.sql -C
```

**Using SSMS:**
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open `Scripts/DatabaseSetup.sql`
4. Execute the script (F5)

This creates:
- `BookstoreDB` database
- `Books` table with columns: `Id`, `Title`, `Author`, `Price`, `PublicationYear`, `Genre`
- 5 stored procedures for CRUD operations

### 3. Configure Connection String
Update [`appsettings.json`](BookstoreApp/appsettings.json:1) if your SQL Server instance differs:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookstoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Restore Dependencies & Run
```bash
dotnet restore
dotnet run
```

The application will be available at **http://localhost:5000**

---

## 🗄️ Database Schema

### Books Table
| Column | Type | Description |
|--------|------|-------------|
| `Id` | `INT` (PK, Identity) | Unique book identifier |
| `Title` | `NVARCHAR(200)` | Book title |
| `Author` | `NVARCHAR(150)` | Author name |
| `Price` | `DECIMAL(10,2)` | Book price |
| `PublicationYear` | `INT` | Year of publication |
| `Genre` | `NVARCHAR(100)` | Book genre/category |

### Stored Procedures
| Procedure | Parameters | Description |
|-----------|------------|-------------|
| `sp_GetAllBooks` | None | Returns all books |
| `sp_GetBookById` | `@Id INT` | Returns a single book |
| `sp_AddBook` | `@Title, @Author, @Price, @PublicationYear, @Genre` | Inserts a new book |
| `sp_UpdateBook` | `@Id, @Title, @Author, @Price, @PublicationYear, @Genre` | Updates a book |
| `sp_DeleteBook` | `@Id INT` | Deletes a book |

---

## 🛠️ Technologies Used

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core MVC** | Web application framework |
| **ADO.NET** | Database access (SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter) |
| **Microsoft.Data.SqlClient** | SQL Server data provider |
| **SQL Server** | Relational database |
| **Bootstrap 5** | Frontend CSS framework (CDN) |
| **jQuery Validation** | Client-side form validation |
| **Razor Views** | Server-side rendering engine |

---

## 📝 Key Implementation Details

### Parameterized Queries (SQL Injection Prevention)
```csharp
string query = "SELECT * FROM Books WHERE Id = @Id";
using var command = new SqlCommand(query, connection);
command.Parameters.AddWithValue("@Id", id);
```

### Stored Procedure Execution
```csharp
using var command = new SqlCommand("sp_GetAllBooks", connection);
command.CommandType = CommandType.StoredProcedure;
using var reader = command.ExecuteReader();
```

### DataSet with SqlDataAdapter
```csharp
using var adapter = new SqlDataAdapter("SELECT * FROM Books", connection);
var dataSet = new DataSet();
adapter.Fill(dataSet, "Books");
```

---

## 📄 License

This project is created for educational purposes as part of an ASP.NET training assignment.

---

## 👤 Author

Built as a practice assignment demonstrating ADO.NET data access patterns in ASP.NET Core MVC.