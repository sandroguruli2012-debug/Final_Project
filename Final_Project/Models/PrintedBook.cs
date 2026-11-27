namespace Final_Project.Models
{
    public class PrintedBook : Book
    {
        public PrintedBook(string title, string author, int year, Genre genre)
            : base(title, author, year, genre)
        {
        }

        public override string BookType => "ჩვეულებრივი";

        public override string Details => "-";
    }
}