using BookstoreDemo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookstoreDemo.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(
        IOptions<BookStoreDatabaseSetting> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAllBooksAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> FindBookAsync(String isbn) =>
        await _booksCollection.Find(x => x.ISBN == isbn).FirstOrDefaultAsync();

    public async Task CreateBookAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string isbn, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.ISBN == isbn, updatedBook);
        
    public async Task RemoveAsync(String isbn) =>
        await _booksCollection.DeleteOneAsync(x => x.ISBN == isbn);
}