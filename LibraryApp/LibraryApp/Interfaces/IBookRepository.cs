using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task UpdateAsync(Guid id, Func<Book, Book> update);
    Task<bool> RemoveByIdAsync(Guid id);
}