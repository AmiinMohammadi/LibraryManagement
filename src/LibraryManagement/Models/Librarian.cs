namespace LibraryManagement.Models
{
    public class Librarian(int id, string firstName, string lastName, decimal salary) : Person(id, firstName, lastName)
    {
        public DateTime HireDate { get; init; } = DateTime.UtcNow;
        public decimal Salary { get; set; } = salary;
    }
}