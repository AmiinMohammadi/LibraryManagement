using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly Dictionary<int, Member> _members = new Dictionary<int, Member>();


    public IEnumerable<Member> GetAll()
    {
        return new List<Member>(_members.Values);
    }

    public IEnumerable<Member> Search(string fullName)
    {
        foreach (var member in _members.Values)
        {
            if (member.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase))
            {
                yield return member;
            }
        }
    }

    public Member? GetById(int id)
    {
        return _members.GetValueOrDefault(id);
    }

    public void Add(Member member)
    {
        _members.Add(member.Id, member);
    }

    public bool Delete(int id)
    {
        return _members.Remove(id);
    }
}