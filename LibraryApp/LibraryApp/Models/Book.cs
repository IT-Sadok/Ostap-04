namespace LibraryApp.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public int YearOfPublication { get; set; }
    public BookState BookState { get; set; } = BookState.Available;

    public override string ToString()
    {
        return $"{Id} - {Title} - {AuthorName} - {YearOfPublication}";
    }
}