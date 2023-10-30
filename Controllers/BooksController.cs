using BookstoreDemo.Models;
using BookstoreDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) =>
        _booksService = booksService;

    [HttpGet]
    public async Task<List<Book>> GetAllBooks() =>
        await _booksService.GetAllBooksAsync();

    [HttpGet("{isbn:length(13)}")]
    public async Task<ActionResult<Book>> FindBook(string isbn)
    {
        var book = await _booksService.FindBookAsync(isbn);

        if (book is null)
        {
            var message = string.Format("The book ISBN number can't be found");
            return NotFound(message);
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookPost(Book newBook)
    {   try
        {
            await _booksService.CreateBookAsync(newBook);
        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error while inserting data into the database");
        }
        return CreatedAtAction(nameof(FindBook), new { isbn = newBook.ISBN }, newBook);
    }

    [HttpPut("{isbn:length(13)}")]
    public async Task<IActionResult> UpdateBook(string isbn, Book updatedBook)
    {
        try
        {
            var book = await _booksService.FindBookAsync(isbn);

            if (book is null)
            {
                var message = string.Format("The book ISBN number can't be found");
                return NotFound(message);
            }
            updatedBook.Id = book.Id;
            await _booksService.UpdateAsync(isbn, updatedBook);
        }
        catch(Exception)
        {
             return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating book to the database");
        }
        
        return NoContent();
    }

    [HttpDelete("{isbn:length(13)}")]
    public async Task<IActionResult> Delete(string isbn)
    {
        var book = await _booksService.FindBookAsync(isbn);

        if (book is null)
        {
            var message = string.Format("The book ISBN number can't be found");
            return NotFound(message);
        }
        await _booksService.RemoveAsync(isbn);

        return NoContent();
    }   
}