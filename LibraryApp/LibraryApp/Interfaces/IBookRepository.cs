using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookRepository
{
    IReadOnlyList<Book> GetAll();
    Book? GetById(Guid id);
    void Add(Book book);
    void Update(Book book);
    bool RemoveById(Guid id);
}