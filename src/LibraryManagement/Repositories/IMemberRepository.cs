using LibraryManagement.Models;
namespace LibraryManagement.Repositories;

public interface IMemberRepository
{
    IEnumerable<Member> GetAll();
    IEnumerable<Member> Search(string fullName);
    Member? GetById(int id);
    void Add(Member member);

    bool Delete(int id);
}