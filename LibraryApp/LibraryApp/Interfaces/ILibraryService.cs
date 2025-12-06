using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService: IBookSearchService, ILibraryManagement
{
    IReadOnlyList<BookModel> GetAllBooks();
    void AddBook(Book book);
    bool RemoveById(Guid id);
}