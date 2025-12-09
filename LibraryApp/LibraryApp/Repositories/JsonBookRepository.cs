using System.Collections.Concurrent;
using System.Text.Json;
using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Repositories;

public class JsonBookRepository : IBookRepository
{
    private readonly string _jsonFilePath;
    private readonly List<Book> _books;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    private JsonBookRepository(string jsonFilePath, List<Book> books)
    {
        _jsonFilePath = jsonFilePath;
        _books = books;
    }

    public static async Task<JsonBookRepository> CreateAsync(string jsonFilePath)
    {
        var books = await ReadFromFileAsync(jsonFilePath);
        return new JsonBookRepository(jsonFilePath, books);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync()
    {
        // to receive up-to-date data
        // maybe better to use ConcurrentDictionary<Guid, Book>
        await _semaphoreSlim.WaitAsync();
        try
        {
            return _books.AsReadOnly();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task AddAsync(Book book)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _books.Add(book);
            await SaveAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task UpdateAsync(Guid id, Func<Book, Book> update)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var index = _books.FindIndex(b => b.Id == id);
            if (index == -1)
                return;

            _books[index] = update(_books[index]);
            await SaveAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<bool> RemoveByIdAsync(Guid id)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return false;
            }

            if (!_books.Remove(book))
            {
                return false;
            }

            await SaveAsync();

            return true;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private static async Task<List<Book>> ReadFromFileAsync(string jsonFilePath)
    {
        try
        {
            if (!File.Exists(jsonFilePath))
            {
                await File.WriteAllTextAsync(jsonFilePath, "[]");
                return [];
            }

            var text = await File.ReadAllTextAsync(jsonFilePath);
            var books = JsonSerializer.Deserialize<List<Book>>(text, JsonSerializerOptions);

            return books ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
            return [];
        }
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_books, JsonSerializerOptions);
        await File.WriteAllTextAsync(_jsonFilePath, json);
    }
}