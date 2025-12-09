using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookRepository
{
    IReadOnlyList<Book> GetAll();
    Book? GetById(Guid id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task<bool> RemoveByIdAsync(Guid id);
}