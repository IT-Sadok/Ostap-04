namespace LibraryApp.Models;

public class BookFilterModel
{
    public string? AuthorName { get; set; }
    public string? Title { get; set; }
    public bool? Available { get; set; }
}