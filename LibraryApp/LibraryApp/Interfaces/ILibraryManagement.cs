namespace LibraryApp.Interfaces;

public interface ILibraryManagement
{
    void BorrowBook(Guid id);
    bool ReturnBook(Guid id);
}