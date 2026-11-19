using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public class InMemoryBookRepository : IBookRepository
{
    private readonly Dictionary<int, Book> _books = new Dictionary<int, Book>();



    public IEnumerable<Book> GetAll()
    {
        return new List<Book>(_books.Values);
    }

    public IEnumerable<Book> Search(string title)
    {
        foreach (var book in _books.Values)
        {
            if (book.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            {
                yield return book;
            }
        }
    }


    public Book? GetById(int id)
    {
        return _books.GetValueOrDefault(id);
    }

    public void Add(Book book)
    {
        _books.Add(book.Id, book);
    }

    public bool Delete(int id)
    {
        return _books.Remove(id);
    }
}