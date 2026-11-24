namespace Final_Project.Models
{
    public class PrintedBook : Book
    {
        public PrintedBook(string title, string author, int year)
            : base(title, author, year)
        {
        }

        public override string BookType => "ჩვეულებრივი";

        public override string Details => "-";
    }
}