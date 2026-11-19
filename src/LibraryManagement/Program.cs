using LibraryManagement.Models;
using LibraryManagement.Repositories;


var book2 = new Book(2, "Financial", "Reza", 1999);
var book1 = new Book(1, "Tech", "Qolam", 2005);
var book3 = new Book(3, "Development", "Ali", 1999);
IBookRepository bookRepository = new InMemoryBookRepository();
bookRepository.Add(book1);
bookRepository.Add(book2);
bookRepository.Add(book3);
Console.WriteLine("———————————————————————————————————————");
Console.WriteLine("All Bookss::");
PrintAllBook(bookRepository);
Console.WriteLine("————————————————");

Console.Write("Deleting Book1 => ");
Console.WriteLine(bookRepository.Delete(book1.Id) ? "OK" : "Failed");
Console.WriteLine("————————————————");

Console.WriteLine("All Bookss::");
PrintAllBook(bookRepository);
Console.WriteLine("————————————————");

Console.Write("Getting title of book 3 => ");
var resultBook = bookRepository.GetById(3);
Console.WriteLine(resultBook is null ? "not found" : $"{resultBook.Title}");
Console.WriteLine("————————————————");
Console.WriteLine("Searching for 'dev' bookRepository:");
foreach (var item in bookRepository.Search("dev"))
    Console.WriteLine($"{item.Id} - {item.Title}");

Console.WriteLine("———————————————————————————————————————");

var member1 = new Member(1, "Ali", "Yari");
var member2 = new Member(2, "Pari", "Pooyani");
var member3 = new Member(3, "Jim", "Yari");
IMemberRepository memberRepository = new InMemoryMemberRepository();
memberRepository.Add(member1);
memberRepository.Add(member2);
memberRepository.Add(member3);
Console.WriteLine("All Members:");
PrintAllMember(memberRepository);
Console.WriteLine("————————————————");

Console.Write("Deleting Member1 => ");
Console.WriteLine(memberRepository.Delete(member1.Id) ? "OK" : "Failed");
Console.WriteLine("————————————————");

Console.WriteLine("All Members:");
PrintAllMember(memberRepository);

Console.WriteLine("————————————————");

Console.Write("Getting name of Member 3 => ");
var resultMember = memberRepository.GetById(3);
Console.WriteLine(resultMember is null ? "not found" : $"{resultMember.FullName}");
Console.WriteLine("————————————————");
Console.WriteLine("Searching for 'i' MemberRepository:");
foreach (var item in memberRepository.Search("i"))
    Console.WriteLine($"{item.Id} - {item.FullName}");

Console.WriteLine("———————————————————————————————————————");
return;

void PrintAllMember(IMemberRepository repo)
{
    foreach (var item in repo.GetAll())
        Console.WriteLine($"{item.Id} - {item.FullName}");
}

void PrintAllBook(IBookRepository repo)
{
    foreach (var item in repo.GetAll())
        Console.WriteLine($"{item.Id} - {item.Title}");
}