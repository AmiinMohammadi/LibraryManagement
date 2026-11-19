using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

internal interface IBookRepository
{
    public IEnumerable<Book> GetAll();
    public IEnumerable<Book> Search(string title);
    public Book? GetById(int id);
    public void Add(Book book);

    public bool Delete(int id);
}