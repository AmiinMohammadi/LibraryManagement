namespace LibraryManagement.Models;

public class Member(int id, string firstName, string lastName) : Person(id, firstName, lastName)
{
    public DateTime MembershipDate { get; init; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}