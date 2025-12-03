using System.Text.Json;
using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Repositories;

public class JsonBookRepository : IBookRepository
{
    private readonly string _jsonFilePath;
    private readonly List<Book> _books;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public JsonBookRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
        _books = ReadFromFile();
    }

    public IReadOnlyList<Book> GetAll() => _books.AsReadOnly();

    public Book? GetById(Guid id)
        => _books.FirstOrDefault(b => b.Id == id);

    public void Add(Book book)
    {
        _books.Add(book);
        Save();
    }

    public void Update(Book book)
    {
        var idx = _books.FindIndex(b => b.Id == book.Id);
        if (idx == -1)
        {
            throw new ArgumentException($"Book with id {book.Id} not found");
        }

        _books[idx] = book;
        Save();
    }

    public bool RemoveById(Guid id)
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

        Save();

        return true;
    }

    private List<Book> ReadFromFile()
    {
        try
        {
            var text = File.ReadAllText(_jsonFilePath);
            var books = JsonSerializer.Deserialize<List<Book>>(text, _jsonSerializerOptions);

            return books ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return [];
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_books, _jsonSerializerOptions);
        File.WriteAllText(_jsonFilePath, json);
    }
}