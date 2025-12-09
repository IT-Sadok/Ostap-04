using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService: IBookSearchService, ILibraryManagement
{
    IReadOnlyList<BookModel> GetAllBooks();
    Task AddBookAsync(Book book);
    Task<bool> RemoveByIdAsync(Guid id);
    Task EditBookAsync(Guid id, string title, string authorName, int yearOfPublication);
}