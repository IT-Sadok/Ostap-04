namespace LibraryApp.Models;

public record BookModel(
    Guid Id,
    string Title,
    string AuthorName,
    int YearOfPublication,
    BookState BookState = BookState.Available
);