using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Repositories;
using LibraryApp.Services;

namespace LibraryApp;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        const string filePath = "../../../Data/library.json";
        IBookRepository bookRepository = await JsonBookRepository.CreateAsync(filePath);
        ILibraryService libraryService = new LibraryService(bookRepository);

        var app = new ConsoleLibrary(libraryService);
        await app.Run();
    }
}