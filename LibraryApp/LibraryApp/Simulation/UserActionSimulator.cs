using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Utils;

namespace LibraryApp.Simulation;

public class UserActionSimulator
{
    private readonly ILibraryService _libraryService;
    private readonly BookSeeder _bookSeeder;

    public UserActionSimulator(ILibraryService libraryService)
    {
        _libraryService = libraryService;
        _bookSeeder = new BookSeeder(_libraryService);
    }

    public async Task SimulateUserActionAsync(int taskCount)
    {
        await SeedBooks(taskCount);

        var tasks = new List<Task>();
        for (int i = 0; i < taskCount; i++)
        {
            var index = i;
            tasks.Add(Task.Run(async () =>
                {
                    var allBooks = await _libraryService.GetAllBooksAsync();
                    var ids = allBooks.Select(b => b.Id).ToList();
                    if (!ids.Any()) return;
                    var id = ids[Random.Shared.Next(ids.Count)];
                    var action = Random.Shared.Next(0, 3);
                    switch (action)
                    {
                        case 0:
                            await _libraryService.AddBookAsync(new Book(Id: Guid.NewGuid(), Title: $"Book {index}",
                                AuthorName: $"Author {index}",
                                YearOfPublication: Random.Shared.Next(1850, DateTime.Today.Year)));
                            break;
                        case 1:
                            await _libraryService.RemoveByIdAsync(id);
                            break;
                        case 2:
                            await _libraryService.EditBookAsync(id, $"Book {index}",
                                $"Author {index}", Random.Shared.Next(1850, DateTime.Today.Year));
                            break;
                    }
                }
            ));
        }

        await Task.WhenAll(tasks);
    }

    private async Task SeedBooks(int count)
    {
        var allBooks = await _libraryService.GetAllBooksAsync();
        if (allBooks.Count <= count)
        {
            await _bookSeeder.AddRandomBooksAsync(count);
        }
    }
}