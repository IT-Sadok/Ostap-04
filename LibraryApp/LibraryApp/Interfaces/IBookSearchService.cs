using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookSearchService
{
    IEnumerable<BookModel> FindBooks(BookFilterModel filterModel);
}