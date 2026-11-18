namespace LibraryManagement.Models
{
    public abstract class Person(int id, string firstName, string lastName)
    {
        public int Id { get; init; } = id;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;

        public string FullName => $"{FirstName} {LastName}";
    }
}