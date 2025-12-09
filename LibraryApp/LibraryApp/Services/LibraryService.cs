using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class LibraryService : ILibraryService
{
    private readonly IBookRepository _bookRepository;

    public LibraryService(IBookRepository jsonBookRepository)
    {
        _bookRepository = jsonBookRepository;
    }

    public async Task<IReadOnlyList<BookModel>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(ToBookModel).ToList();
    }

    public async Task AddBookAsync(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (book.Id == Guid.Empty)
        {
            throw new ArgumentException("Book id cannot be empty");
        }


        if (await _bookRepository.GetByIdAsync(book.Id) is not null)
        {
            throw new ArgumentException("Book already exists");
        }

        await _bookRepository.AddAsync(book);
    }

    public async Task<bool> RemoveByIdAsync(Guid id)
    {
        return await _bookRepository.RemoveByIdAsync(id);
    }

    public async Task EditBookAsync(Guid id, string title, string authorName, int yearOfPublication)
    {
        await _bookRepository.UpdateAsync(id, b =>
            b with
            {
                Title = title,
                AuthorName = authorName,
                YearOfPublication = yearOfPublication
            });
    }

    public async Task<IEnumerable<BookModel>> FindBooksAsync(BookFilterModel filterModel)
    {
        ArgumentNullException.ThrowIfNull(filterModel);

        var books = await _bookRepository.GetAllAsync();

        var query = books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filterModel.AuthorName))
        {
            query = query.Where(b =>
                b.AuthorName.Contains(filterModel.AuthorName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filterModel.Title))
        {
            query = query.Where(b =>
                b.Title.Contains(filterModel.Title, StringComparison.OrdinalIgnoreCase));
        }

        if (filterModel.Available == true)
        {
            query = query.Where(b => b.BookState == BookState.Available);
        }

        return query.Select(ToBookModel).ToList();
    }

    public async Task<bool> BorrowBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);
        if (book.BookState == BookState.CheckedOut)
        {
            return false;
        }

        await _bookRepository.UpdateAsync(id, b =>
            b with
            {
                BookState = BookState.CheckedOut
            });

        return true;
    }

    public async Task<bool> ReturnBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);

        if (book.BookState == BookState.Available)
        {
            return false;
        }

        await _bookRepository.UpdateAsync(id, b =>
            b with
            {
                BookState = BookState.Available
            });

        return true;
    }

    private async Task<Book> GetBookByIdAsync(Guid id) =>
        await _bookRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Book not found");

    private static BookModel ToBookModel(Book book) => new(
        book.Id,
        book.Title,
        book.AuthorName,
        book.YearOfPublication,
        book.BookState
    );
}