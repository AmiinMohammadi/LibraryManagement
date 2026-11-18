namespace LibraryManagement.Models
{
    public class Book(int id, string title, string author, int year)
    {
        public int Id { get; init; } = id;
        public string Title { get; set; } = title;
        public string Author { get; set; } = author;
        public bool IsAvailable { get; set; } = true;
        public int Year { get; set; } = year;
    }
}