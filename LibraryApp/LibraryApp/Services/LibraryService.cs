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

    public IReadOnlyList<BookModel> GetAllBooks() => _jsonBookRepository.GetAll().Select(ToBookModel).ToList();

    public async Task AddBookAsync(Book book)
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

        await _jsonBookRepository.AddAsync(book);
    }

    public async Task<bool> RemoveByIdAsync(Guid id)
    {
        return await _jsonBookRepository.RemoveByIdAsync(id);
    }

    public async Task EditBookAsync(Guid id, string title, string authorName, int yearOfPublication)
    {
        var book = GetBookById(id);

        var updatedBook = book with
        {
            Title = title,
            AuthorName = authorName,
            YearOfPublication = yearOfPublication
        };

        await _jsonBookRepository.UpdateAsync(updatedBook);
    }

    public IEnumerable<BookModel> FindBooks(BookFilterModel filterModel)
    {
        ArgumentNullException.ThrowIfNull(filterModel);

        var query = _jsonBookRepository.GetAll().AsQueryable();


        if (!string.IsNullOrWhiteSpace(filterModel.AuthorName))
        {
            query = query.Where(b => b.AuthorName.Contains(filterModel.AuthorName));
        }

        if (!string.IsNullOrWhiteSpace(filterModel.Title))
        {
            query = query.Where(b => b.Title.Contains(filterModel.Title));
        }

        if (filterModel.Available == true)
        {
            query = query.Where(b => b.BookState == BookState.Available);
        }

        return query.AsEnumerable().Select(ToBookModel);
    }

    public async Task<bool> BorrowBookAsync(Guid id)
    {
        var book = GetBookById(id);
        if (book.BookState == BookState.CheckedOut)
        {
            return false;
        }

        var updatedBook = book with
        {
            BookState = BookState.CheckedOut
        };

        await _jsonBookRepository.UpdateAsync(updatedBook);

        return true;
    }

    public async Task<bool> ReturnBookAsync(Guid id)
    {
        var book = GetBookById(id);

        if (book.BookState == BookState.Available)
        {
            return false;
        }

        var updatedBook = book with
        {
            BookState = BookState.Available
        };

        await _jsonBookRepository.UpdateAsync(updatedBook);

        return true;
    }

    private Book GetBookById(Guid id) =>
        _jsonBookRepository.GetById(id) ?? throw new KeyNotFoundException("Book not found");

    private static BookModel ToBookModel(Book book) => new(
        book.Id,
        book.Title,
        book.AuthorName,
        book.YearOfPublication,
        book.BookState
    );
}