using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Utils;

public class BookSeeder
{
    private readonly ILibraryService _libraryService;

    private readonly List<string> _sampleTitles =
    [
        "The Adventure", "Mystery of the Night", "C# in Depth",
        "Programming Challenges", "Secrets of the Universe", "The Last Stand",
        "Parallel Worlds", "Legends of Code", "Future Tech", "Hidden Truths"
    ];

    private readonly List<string> _sampleAuthors =
    [
        "John Smith", "Jane Doe", "Alice Johnson", "Bob Brown",
        "Charlie Davis", "Eve Wilson", "Frank Miller", "Grace Lee"
    ];

    public BookSeeder(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task AddRandomBooksAsync(int count = 100)
    {
        for (int i = 0; i < count; i++)
        {
            var title = _sampleTitles[Random.Shared.Next(_sampleTitles.Count)] + " " + Random.Shared.Next(1, 1000);
            var author = _sampleAuthors[Random.Shared.Next(_sampleAuthors.Count)];
            var year = Random.Shared.Next(1900, DateTime.Now.Year + 1);

            var book = new Book(
                Guid.NewGuid(),
                title,
                author,
                year
            );

            try
            {
                await _libraryService.AddBookAsync(book);
                Console.WriteLine($"Added: {title} by {author} ({year})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add book: {ex.Message}");
            }
        }
    }
}