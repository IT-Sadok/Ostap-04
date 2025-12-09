using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Simulation;

namespace LibraryApp;

public class ConsoleLibrary
{
    private readonly ILibraryService _libraryService;
    private readonly UserActionSimulator _userActionSimulator;

    public ConsoleLibrary(ILibraryService libraryService)
    {
        _libraryService = libraryService;
        _userActionSimulator = new UserActionSimulator(libraryService);
    }

    private void ShowMenu()
    {
        Console.WriteLine("====== Library Menu ======");
        Console.WriteLine("1. Add book");
        Console.WriteLine("2. Remove book by id");
        Console.WriteLine("3. Filter books (Author, Title, Availability)");
        Console.WriteLine("4. Show all books");
        Console.WriteLine("5. Borrow book");
        Console.WriteLine("6. Return book");
        Console.WriteLine("7. Edit book");
        Console.WriteLine("88. Simulate users actions");
        Console.WriteLine("0. Exit");
        Console.WriteLine("*. Show menu");
        Console.WriteLine("========================");
    }

    public async Task Run()
    {
        ShowMenu();

        while (true)
        {
            Console.Write("Choose an option(digit): ");
            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await AddBookAsync();
                    break;
                case "2":
                    await RemoveBookAsync();
                    break;
                case "3":
                    await FilterBooksAsync();
                    break;
                case "4":
                    await ShowAllBooksAsync();
                    break;
                case "5":
                    await BorrowBookAsync();
                    break;
                case "6":
                    await ReturnBookAsync();
                    break;
                case "7":
                    await EditBookAsync();
                    break;
                case "88":
                    await SimulateUserActionsAsync();
                    break;
                case "0":
                    return;
                case "*":
                    ShowMenu();
                    break;
                default:
                    Console.WriteLine("Invalid input,  please try again:");
                    break;
            }

            Console.WriteLine();
        }
    }

    private async Task AddBookAsync()
    {
        Console.WriteLine("====== Book adding ======");
        Console.Write("Enter title: ");
        var title = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter author: ");
        var author = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter year of publication: ");

        if (!int.TryParse(Console.ReadLine(), out var year))
        {
            Console.WriteLine("Invalid input, year is 0");
            year = 0;
        }

        var book = new Book(
            Guid.NewGuid(),
            title,
            author,
            year
        );

        try
        {
            await _libraryService.AddBookAsync(book);
            Console.WriteLine("Book added");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while adding book: " + e.Message);
        }
    }

    private async Task RemoveBookAsync()
    {
        Console.WriteLine("====== Removing book ======");
        Console.Write("Enter id: ");
        var id = Console.ReadLine() ?? string.Empty;

        if (!Guid.TryParse(id, out var bookId))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        Console.WriteLine(await _libraryService.RemoveByIdAsync(bookId)
            ? "Book removed"
            : "Book with such id is not found");
    }

    private async Task FilterBooksAsync()
    {
        Console.WriteLine("====== Filtering books ======");
        Console.Write("Enter author name: ");
        var authorName = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter title: ");
        var title = Console.ReadLine() ?? string.Empty;
        Console.Write("Search only available books (y/n): ");
        var available = Console.ReadLine() == "y";

        var filterModel = new BookFilterModel
        {
            AuthorName = authorName,
            Title = title,
            Available = available
        };

        var books = await _libraryService.FindBooksAsync(filterModel);
        PrintBooks(books);
    }

    private async Task ShowAllBooksAsync()
    {
        var books = await _libraryService.GetAllBooksAsync();
        PrintBooks(books);
    }

    private async Task BorrowBookAsync()
    {
        Console.WriteLine("====== Borrowing book ======");
        Console.WriteLine("Write id of book you want to borrow: ");
        var id = Console.ReadLine() ?? string.Empty;

        if (!Guid.TryParse(id, out var bookId))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        try
        {
            Console.WriteLine(await _libraryService.BorrowBookAsync(bookId)
                ? "Book borrowed"
                : "Book was already borrowed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while borrowing book: " + e.Message);
        }
    }

    private async Task ReturnBookAsync()
    {
        Console.WriteLine("====== Returning book ======");
        Console.WriteLine("Write id of book you want to return: ");
        var id = Console.ReadLine() ?? string.Empty;

        if (!Guid.TryParse(id, out var bookId))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        try
        {
            Console.WriteLine(await _libraryService.ReturnBookAsync(bookId)
                ? "Book returned"
                : "Book is already returned");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while returning book: " + e.Message);
        }
    }

    private async Task EditBookAsync()
    {
        Console.WriteLine("====== Editing book ======");
        Console.WriteLine("Write id of book you want to edit: ");
        var id = Console.ReadLine() ?? string.Empty;
        if (!Guid.TryParse(id, out var bookId))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        Console.Write("Enter new author name: ");
        var authorName = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter new title: ");
        var title = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter new year of publication: ");

        if (!int.TryParse(Console.ReadLine(), out var year))
        {
            Console.WriteLine("Invalid input, year is 0");
            year = 0;
        }

        try
        {
            await _libraryService.EditBookAsync(bookId, authorName, title, year);
            Console.WriteLine("Book edited");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while editing book: " + e.Message);
        }
    }

    private static void PrintBooks(IEnumerable<BookModel> books)
    {
        var enumerable = books.ToList();
        if (enumerable.Count == 0)
        {
            Console.WriteLine("Nothing found");
        }

        var num = 1;
        foreach (var book in enumerable)
        {
            Console.WriteLine($"{num++}: {book}");
        }
    }

    private async Task SimulateUserActionsAsync()
    {
        Console.WriteLine("====== Simulating user actions ======");
        Console.WriteLine("Enter number of tasks you want to simulate: ");
        if (!int.TryParse(Console.ReadLine(), out var taskCount))
        {
            Console.WriteLine("Invalid input, task count is 100");
            taskCount = 100;
        }

        await _userActionSimulator.SimulateUserActionAsync(taskCount);
    }
}