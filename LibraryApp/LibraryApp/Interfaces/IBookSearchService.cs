using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookSearchService
{
    IEnumerable<Book> FindBooksByAuthor(string author);
    IEnumerable<Book> FindBooksByTitle(string title);
    IEnumerable<Book> GetAllAvailableBooks();
}