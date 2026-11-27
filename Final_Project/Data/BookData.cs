using Final_Project.Models;

namespace Final_Project.Data
{
    public class BookData
    {
        public string Kind { get; set; } = "";
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Year { get; set; }
        public Genre Genre { get; set; }
        public double FileSizeMb { get; set; }
        public string? BorrowedBy { get; set; }
        public DateTime? BorrowedDate { get; set; }

        public static BookData FromBook(Book book)
        {
            BookData data = new BookData
            {
                Kind = book is EBook ? nameof(EBook) : nameof(PrintedBook),
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Genre = book.Genre,
                BorrowedBy = book.BorrowedBy,
                BorrowedDate = book.BorrowedDate
            };

            if (book is EBook ebook)
                data.FileSizeMb = ebook.FileSizeMb;

            return data;
        }

        public Book ToBook()
        {
            Book book;

            if (Kind == nameof(EBook))
                book = new EBook(Title, Author, Year, Genre, FileSizeMb);
            else
                book = new PrintedBook(Title, Author, Year, Genre);

            if (BorrowedBy != null && BorrowedDate != null)
                book.RestoreBorrow(BorrowedBy, BorrowedDate.Value);

            return book;
        }
    }
}