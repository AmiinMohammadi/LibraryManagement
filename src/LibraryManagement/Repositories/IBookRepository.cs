using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    IEnumerable<Book> Search(string title);
    Book? GetById(int id);
    void Add(Book book);

    bool Delete(int id);
}