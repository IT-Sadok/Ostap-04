using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class LibraryService : ILibraryService
{
    private readonly IBookRepository _jsonBookRepository;

    public LibraryService(IBookRepository jsonBookRepository)
    {
        _jsonBookRepository = jsonBookRepository;
    }

    public IReadOnlyList<Book> GetAllBooks() => _jsonBookRepository.GetAll();

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (book.Id == Guid.Empty)
        {
            throw new ArgumentException("Book id cannot be empty");
        }

        if (_jsonBookRepository.GetById(book.Id) is not null)
        {
            throw new ArgumentException("Book already exists");
        }

        _jsonBookRepository.Add(book);
    }

    public bool RemoveById(Guid id)
    {
        return _jsonBookRepository.RemoveById(id);
    }

    public IEnumerable<Book> FindBooksByAuthor(string author)
    {
        if (!string.IsNullOrEmpty(author))
        {
            return _jsonBookRepository.GetAll().Where(b => b.AuthorName.Contains(author));
        }

        return [];
    }

    public IEnumerable<Book> FindBooksByTitle(string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            return _jsonBookRepository.GetAll().Where(b => b.Title.Contains(title));
        }

        return [];
    }

    public IEnumerable<Book> GetAllAvailableBooks()
    {
        return _jsonBookRepository.GetAll().Where(b => b.BookState == BookState.Available);
    }

    public void BorrowBook(Guid id)
    {
        var book = GetBookById(id);
        if (book.BookState == BookState.CheckedOut)
        {
            throw new InvalidOperationException("Book already checked-out");
        }

        book.BookState = BookState.CheckedOut;
        _jsonBookRepository.Update(book);
    }

    public bool ReturnBook(Guid id)
    {
        var book = GetBookById(id);

        if (book.BookState == BookState.Available)
        {
            return false;
        }

        book.BookState = BookState.Available;
        _jsonBookRepository.Update(book);

        return true;
    }

    private Book GetBookById(Guid id) =>
        _jsonBookRepository.GetById(id) ?? throw new KeyNotFoundException("Book not found");
}