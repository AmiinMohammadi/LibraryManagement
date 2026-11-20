using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;

// ===== Setup =====
var bookRepo = new InMemoryBookRepository();
var memberRepo = new InMemoryMemberRepository();

var book1 = new Book(1, "Clean Code", "Robert Martin", 2008);
var book2 = new Book(2, "The Pragmatic Programmer", "Hunt & Thomas", 1999);
var book3 = new Book(3, "Design Patterns", "GoF", 1994);
var book4 = new Book(4, "Refactoring", "Martin Fowler", 1999);

bookRepo.Add(book1);
bookRepo.Add(book2);
bookRepo.Add(book3);
bookRepo.Add(book4);

var activeMember = new Member(1, "Ali", "Yari");
var inactiveMember = new Member(2, "Pari", "Pooyani") { IsActive = false };

memberRepo.Add(activeMember);
memberRepo.Add(inactiveMember);

var library = new LibraryService(bookRepo, memberRepo);

// ===== Helper =====
void PrintResult(string scenario, Result result)
{
    var status = result.IsSuccess ? "✅ Success" : $"❌ Failure: {result.Error}";
    Console.WriteLine($"  {scenario}: {status}");
}

Console.WriteLine("===== LibraryService Smoke Test =====");
Console.WriteLine();

// ===== Scenario 1: Borrow available book → Success =====
Console.WriteLine("--- Scenario 1: Borrow available book → Success ---");
var r1 = library.BorrowBook(activeMember.Id, book1.Id);
PrintResult("BorrowBook(active, available book1)", r1);
Console.WriteLine();

// ===== Scenario 2: Borrow same book again → Failure (currently borrowed) =====
Console.WriteLine("--- Scenario 2: Borrow same book again → Failure ---");
var r2 = library.BorrowBook(activeMember.Id, book1.Id);
PrintResult("BorrowBook(active, already-borrowed book1)", r2);
Console.WriteLine();

// ===== Scenario 3: Borrow non-existent book → Failure (NO CRASH!) =====
Console.WriteLine("--- Scenario 3: Borrow non-existent book → Failure (no crash) ---");
var r3 = library.BorrowBook(activeMember.Id, 999);
PrintResult("BorrowBook(active, non-existent book 999)", r3);
Console.WriteLine();

// ===== Scenario 4: Inactive member → Failure =====
Console.WriteLine("--- Scenario 4: Inactive member → Failure ---");
var r4 = library.BorrowBook(inactiveMember.Id, book2.Id);
PrintResult("BorrowBook(inactive, available book2)", r4);
Console.WriteLine();

// ===== Scenario 5: Max 3 books, 4th → Failure =====
Console.WriteLine("--- Scenario 5: Max 3 books, 4th borrow → Failure ---");
library.BorrowBook(activeMember.Id, book2.Id); // 2nd
library.BorrowBook(activeMember.Id, book3.Id); // 3rd
var r5 = library.BorrowBook(activeMember.Id, book4.Id); // 4th → should fail
PrintResult("BorrowBook(active, book4) [4th book]", r5);
Console.WriteLine();

// ===== Scenario 6: Return borrowed book → Success =====
Console.WriteLine("--- Scenario 6: Return borrowed book → Success ---");
var r6 = library.ReturnBook(activeMember.Id, book1.Id);
PrintResult("ReturnBook(active, borrowed book1)", r6);
Console.WriteLine();

// ===== Scenario 7: Return already-returned book → Failure =====
Console.WriteLine("--- Scenario 7: Return already-returned book → Failure ---");
var r7 = library.ReturnBook(activeMember.Id, book1.Id);
PrintResult("ReturnBook(active, already-returned book1)", r7);
Console.WriteLine();

// ===== Scenario 8: GetBorrowedBooks for member with no loans → Success(empty) =====
Console.WriteLine("--- Scenario 8: GetBorrowedBooks (no loans) → Success(empty) ---");
var newMember = new Member(99, "Test", "User");
memberRepo.Add(newMember);
var r8 = library.GetBorrowedBooks(newMember.Id);
Console.WriteLine($"  GetBorrowedBooks(member with no loans): IsSuccess={r8.IsSuccess}, Count={r8.Value.Count}");
Console.WriteLine();

// ===== Scenario 9: GetBorrowedBooks for non-existent member → Failure =====
Console.WriteLine("--- Scenario 9: GetBorrowedBooks (non-existent member) → Failure ---");
var r9 = library.GetBorrowedBooks(9999);
PrintResult("GetBorrowedBooks(non-existent member 9999)", r9);
Console.WriteLine();

// ===== Verification: IsAvailable toggled correctly =====
Console.WriteLine("--- Verification: IsAvailable state ---");
Console.WriteLine($"  book1.IsAvailable (returned): {book1.IsAvailable} (expected: True)");
Console.WriteLine($"  book2.IsAvailable (borrowed): {book2.IsAvailable} (expected: False)");
Console.WriteLine();

// ===== Verification: GetBorrowedBooks returns borrowed list =====
Console.WriteLine("--- Verification: GetBorrowedBooks returns borrowed list ---");
var borrowed = library.GetBorrowedBooks(activeMember.Id);
if (borrowed.IsSuccess)
{
    Console.WriteLine($"  Borrowed books for member {activeMember.Id}: {borrowed.Value.Count}");
    foreach (var b in borrowed.Value)
        Console.WriteLine($"    - {b.Id}: {b.Title}");
}
Console.WriteLine();

Console.WriteLine("===== Smoke Test Complete =====");
