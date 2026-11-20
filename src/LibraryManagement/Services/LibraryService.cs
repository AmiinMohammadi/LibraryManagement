using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class LibraryService(IBookRepository bookRepository, IMemberRepository memberRepository)
{
    private const int MaxBorrowedBooks = 3;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IMemberRepository _memberRepository = memberRepository;

    private readonly Dictionary<int, List<Book>> _loans = new();

    private Result<Member> CheckMember(int memberId)
    {
        var member = _memberRepository.GetById(memberId);
        if (member is null) return Result<Member>.Failure("Member not found in members of library.");

        if (!member.IsActive) return Result<Member>.Failure("Member is not active.");

        return Result<Member>.Success(member);
    }

    private Result<Book> CheckBookBase(int bookId)
    {
        var book = _bookRepository.GetById(bookId);
        if (book is null) return Result<Book>.Failure("Book not found in library.");
        return Result<Book>.Success(book);
    }

    private Result<Book> CheckBookForBorrow(int bookId)
    {
        var book = CheckBookBase(bookId);
        if (book.IsFailure) return book;
        var bookToBorrow = book.Value;
        
        if (!bookToBorrow.IsAvailable) return Result<Book>.Failure("This book is currently borrowed.");
        return Result<Book>.Success(bookToBorrow);
    }
    private Result<Book> CheckBookForReturn(int bookId)
    {
        var book = CheckBookBase(bookId);
        if (book.IsFailure) return book;
        var bookToReturn = book.Value;
        
        if (bookToReturn.IsAvailable) return Result<Book>.Failure("This book is not currently borrowed.");
        return Result<Book>.Success(bookToReturn);
    }
    public Result BorrowBook(int memberId, int bookId)
    {
        var member = CheckMember(memberId);
        if (member.IsFailure) return Result.Failure(member.Error);
        var book = CheckBookForBorrow(bookId);
        if (book.IsFailure) return Result.Failure(book.Error); 
        var bookToBorrow = book.Value;

        if (_loans.TryGetValue(memberId, out var borrowedBooks))
        {
            if (borrowedBooks.Count == MaxBorrowedBooks)
            {
                return Result.Failure(
                    $"The member has reached the maximum {MaxBorrowedBooks} number of borrowed books.");
            }
            borrowedBooks.Add(bookToBorrow);
            bookToBorrow.IsAvailable = false;
            return Result.Success();
        }

        _loans[memberId] = new();
        _loans[memberId].Add(bookToBorrow);
        bookToBorrow.IsAvailable = false;
        return Result.Success();
    }

    public Result ReturnBook(int memberId, int bookId)
    {
        var member = CheckMember(memberId);
        if (member.IsFailure) return Result.Failure(member.Error);
        var book = CheckBookForReturn(bookId);
        if (book.IsFailure) return Result.Failure(book.Error);
        var bookToReturn = book.Value;
        if (_loans.TryGetValue(memberId, out var borrowedBooks))
        {
            if (borrowedBooks.Contains(bookToReturn))
            {
                borrowedBooks.Remove(bookToReturn);
                bookToReturn.IsAvailable = true;
                if (borrowedBooks.Count == 0) _loans.Remove(memberId);
                return Result.Success();
            }

            return Result.Failure("This book is not on the member's list of borrowed books.");
        }

        return Result.Failure("The user is not on the list of those who have borrowed a book.");
    }

    public Result<IReadOnlyList<Book>> GetBorrowedBooks(int memberId)
    {
        var member = CheckMember(memberId);
        if (member.IsFailure) return Result<IReadOnlyList<Book>>.Failure(member.Error);

        if (_loans.TryGetValue(memberId, out var borrowedBooks))
            return Result<IReadOnlyList<Book>>.Success(borrowedBooks.ToList());

        return Result<IReadOnlyList<Book>>.Success(new List<Book>());
    }
}