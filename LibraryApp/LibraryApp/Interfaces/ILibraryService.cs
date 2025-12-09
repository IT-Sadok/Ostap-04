using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService: IBookSearchService, ILibraryManagement
{
    Task<IReadOnlyList<BookModel>> GetAllBooksAsync();
    Task AddBookAsync(Book book);
    Task<bool> RemoveByIdAsync(Guid id);
    Task EditBookAsync(Guid id, string title, string authorName, int yearOfPublication);
}