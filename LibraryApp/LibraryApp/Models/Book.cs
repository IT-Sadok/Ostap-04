namespace LibraryApp.Models;

public record Book(
    Guid Id,
    string Title,
    string AuthorName,
    int YearOfPublication,
    BookState BookState = BookState.Available
);