using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp;

public class ConsoleLibrary
{
    private readonly ILibraryService _libraryService;

    public ConsoleLibrary(ILibraryService libraryService)
    {
        _libraryService = libraryService;
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
        Console.WriteLine("0. Exit");
        Console.WriteLine("*. Show menu");
        Console.WriteLine("========================");
    }

    public void Run()
    {
        ShowMenu();

        while (true)
        {
            Console.Write("Choose an option(digit): ");
            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    FilterBooks();
                    break;
                case "4":
                    ShowAllBooks();
                    break;
                case "5":
                    BorrowBook();
                    break;
                case "6":
                    ReturnBook();
                    break;
                case "7":
                    EditBook();
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

    private void AddBook()
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
            _libraryService.AddBook(book);
            Console.WriteLine("Book added");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while adding book: " + e.Message);
        }
    }

    private void RemoveBook()
    {
        Console.WriteLine("====== Removing book ======");
        Console.Write("Enter id: ");
        var id = Console.ReadLine() ?? string.Empty;

        if (!Guid.TryParse(id, out var bookId))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        Console.WriteLine(_libraryService.RemoveById(bookId) ? "Book removed" : "Book with such id is not found");
    }

    private void FilterBooks()
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

        var books = _libraryService.FindBooks(filterModel);
        PrintBooks(books);
    }

    private void ShowAllBooks()
    {
        var books = _libraryService.GetAllBooks();
        PrintBooks(books);
    }

    private void BorrowBook()
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
            Console.WriteLine(_libraryService.BorrowBook(bookId) ? "Book borrowed" : "Book was already borrowed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while borrowing book: " + e.Message);
        }
    }

    private void ReturnBook()
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
            Console.WriteLine(_libraryService.ReturnBook(bookId) ? "Book returned" : "Book is already returned");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while returning book: " + e.Message);
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

    private void EditBook()
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
            _libraryService.EditBook(bookId, authorName, title, year);
            Console.WriteLine("Book edited");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while editing book: " + e.Message);
        }
    }
}