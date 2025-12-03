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
        Console.WriteLine("3. Search book by author");
        Console.WriteLine("4. Search book by title");
        Console.WriteLine("5. Show all books");
        Console.WriteLine("6. Show all available books");
        Console.WriteLine("7. Borrow book");
        Console.WriteLine("8. Return book");
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
                    SearchBooksByAuthor();
                    break;
                case "4":
                    SearchBooksByTitle();
                    break;
                case "5":
                    ShowAllBooks();
                    break;
                case "6":
                    ShowAvailableBooks();
                    break;
                case "7":
                    BorrowBook();
                    break;
                case "8":
                    ReturnBook();
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

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            AuthorName = author,
            YearOfPublication = year
        };

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

    private void SearchBooksByAuthor()
    {
        Console.WriteLine("====== Searching book by author name ======");
        Console.Write("Enter author name: ");
        var author = Console.ReadLine() ?? string.Empty;
        var books = _libraryService.FindBooksByAuthor(author);
        PrintBooks(books);
    }

    private void SearchBooksByTitle()
    {
        Console.WriteLine("====== Searching book by title ======");
        Console.Write("Enter title: ");
        var title = Console.ReadLine() ?? string.Empty;
        var books = _libraryService.FindBooksByTitle(title);
        PrintBooks(books);
    }

    private void ShowAllBooks()
    {
        var books = _libraryService.GetAllBooks();
        PrintBooks(books);
    }

    private void ShowAvailableBooks()
    {
        var books = _libraryService.GetAllAvailableBooks();
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
            _libraryService.BorrowBook(bookId);
            Console.WriteLine("Book borrowed");
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

    private static void PrintBooks(IEnumerable<Book> books)
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
}