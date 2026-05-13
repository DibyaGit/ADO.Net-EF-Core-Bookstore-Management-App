-- ============================================
-- BookstoreDB Setup Script
-- Run this script in SQL Server Management Studio
-- ============================================

CREATE DATABASE BookstoreDB;
GO

USE BookstoreDB;
GO

-- Books Table
CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    PublicationYear INT NOT NULL,
    Genre NVARCHAR(50) NOT NULL
);
GO

-- ============================================
-- Stored Procedures
-- ============================================

-- Get All Books
CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT Id, Title, Author, Price, PublicationYear, Genre
    FROM Books
    ORDER BY Id;
END
GO

-- Get Book By Id
CREATE PROCEDURE sp_GetBookById
    @Id INT
AS
BEGIN
    SELECT Id, Title, Author, Price, PublicationYear, Genre
    FROM Books
    WHERE Id = @Id;
END
GO

-- Add Book
CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(200),
    @Author NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @PublicationYear INT,
    @Genre NVARCHAR(50)
AS
BEGIN
    INSERT INTO Books (Title, Author, Price, PublicationYear, Genre)
    VALUES (@Title, @Author, @Price, @PublicationYear, @Genre);

    SELECT SCOPE_IDENTITY() AS NewId;
END
GO

-- Update Book
CREATE PROCEDURE sp_UpdateBook
    @Id INT,
    @Title NVARCHAR(200),
    @Author NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @PublicationYear INT,
    @Genre NVARCHAR(50)
AS
BEGIN
    UPDATE Books
    SET Title = @Title,
        Author = @Author,
        Price = @Price,
        PublicationYear = @PublicationYear,
        Genre = @Genre
    WHERE Id = @Id;
END
GO

-- Delete Book
CREATE PROCEDURE sp_DeleteBook
    @Id INT
AS
BEGIN
    DELETE FROM Books
    WHERE Id = @Id;
END
GO