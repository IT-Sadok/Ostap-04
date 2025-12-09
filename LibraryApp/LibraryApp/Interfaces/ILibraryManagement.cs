namespace LibraryApp.Interfaces;

public interface ILibraryManagement
{
    Task<bool> BorrowBookAsync(Guid id);
    Task<bool> ReturnBookAsync(Guid id);
}