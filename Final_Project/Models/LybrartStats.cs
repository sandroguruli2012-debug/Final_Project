namespace Final_Project.Models
{
    public class LibraryStats
    {
        public int TotalBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public int EBooks { get; set; }
        public Book? OldestBook { get; set; }
        public Book? NewestBook { get; set; }
        public Dictionary<Genre, int> ByGenre { get; set; } = new Dictionary<Genre, int>();
        public List<KeyValuePair<string, int>> TopAuthors { get; set; } = new List<KeyValuePair<string, int>>();
    }
}