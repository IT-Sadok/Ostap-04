namespace LibraryApp.Interfaces;

public interface ILibraryManagement
{
    bool BorrowBook(Guid id);
    bool ReturnBook(Guid id);
}