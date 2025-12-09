using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService: IBookSearchService, ILibraryManagement
{
    IReadOnlyList<BookModel> GetAllBooks();
    void AddBook(Book book);
    bool RemoveById(Guid id);
    void EditBook(Guid id, string title, string authorName, int yearOfPublication);
}