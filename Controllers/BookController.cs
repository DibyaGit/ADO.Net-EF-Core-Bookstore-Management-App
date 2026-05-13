using BookstoreApp.Data;
using BookstoreApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers;

public class BookController : Controller
{
    private readonly BookRepository _repository;

    public BookController(BookRepository repository)
    {
        _repository = repository;
    }

    // GET: Book
    public IActionResult Index()
    {
        var books = _repository.GetAllBooks();
        return View(books);
    }

    // GET: Book/Details/5
    public IActionResult Details(int id)
    {
        var book = _repository.GetBookById(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // GET: Book/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Book book)
    {
        if (!ModelState.IsValid)
            return View(book);

        _repository.AddBook(book);
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Edit/5
    public IActionResult Edit(int id)
    {
        var book = _repository.GetBookById(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // POST: Book/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Book book)
    {
        if (id != book.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(book);

        _repository.UpdateBook(book);
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Delete/5
    public IActionResult Delete(int id)
    {
        var book = _repository.GetBookById(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _repository.DeleteBook(id);
        return RedirectToAction(nameof(Index));
    }

    // =============================================
    // Stored Procedure Actions (User Story 3)
    // =============================================

    public IActionResult IndexSP()
    {
        var books = _repository.GetAllBooksViaStoredProcedure();
        return View("Index", books);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateSP(Book book)
    {
        if (!ModelState.IsValid)
            return View("Create", book);

        _repository.AddBookViaStoredProcedure(book);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditSP(int id, Book book)
    {
        if (id != book.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View("Edit", book);

        _repository.UpdateBookViaStoredProcedure(book);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("DeleteSP")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmedSP(int id)
    {
        _repository.DeleteBookViaStoredProcedure(id);
        return RedirectToAction(nameof(Index));
    }

    // =============================================
    // DataSet / DataTable Actions (User Story 4)
    // =============================================

    public IActionResult IndexDataSet()
    {
        var books = _repository.GetAllBooksFromDataSet();
        return View("Index", books);
    }
}