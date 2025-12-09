using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookSearchService
{
    Task<IEnumerable<BookModel>> FindBooksAsync(BookFilterModel filterModel);
}