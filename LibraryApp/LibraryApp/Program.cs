using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Repositories;
using LibraryApp.Services;

namespace LibraryApp;

internal static class Program
{
    private static void Main(string[] args)
    {
        const string filePath = "../../../library.json";
        IBookRepository bookRepository = new JsonBookRepository(filePath);
        ILibraryService libraryService = new LibraryService(bookRepository);

        var app = new ConsoleLibrary(libraryService);
        app.Run();
    }
}