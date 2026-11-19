using LibraryManagement.Models;
namespace LibraryManagement.Repositories;

internal interface IMemberRepository
{
    public IEnumerable<Member> GetAll();
    public IEnumerable<Member> Search(string fullName);
    public Member? GetById(int id);
    public void Add(Member member);

    public bool Delete(int id);
}