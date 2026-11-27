using Final_Project.Exceptions;
using Final_Project.Helpers;

namespace Final_Project.Models
{
    public abstract class Book
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }
        public Genre Genre { get; private set; }
        public string? BorrowedBy { get; private set; }
        public DateTime? BorrowedDate { get; private set; }

        public bool IsAvailable => BorrowedBy == null;

        protected Book(string title, string author, int year, Genre genre)
        {
            if (!Validator.IsValidText(title))
                throw new InvalidBookException("სათაური არ შეიძლება იყოს ცარიელი!");

            if (!Validator.IsValidText(author))
                throw new InvalidBookException("ავტორი არ შეიძლება იყოს ცარიელი!");

            if (!Validator.IsValidYear(year))
                throw new InvalidBookException($"წელი უნდა იყოს 1000-დან {DateTime.Now.Year}-მდე!");

            Title = title.Trim();
            Author = author.Trim();
            Year = year;
            Genre = genre;
        }

        public abstract string BookType { get; }

        public abstract string Details { get; }

        public void Borrow(string username)
        {
            if (!IsAvailable)
                throw new BookAlreadyBorrowedException(Title, BorrowedBy!);

            BorrowedBy = username;
            BorrowedDate = DateTime.Now;
        }

        public void Return()
        {
            BorrowedBy = null;
            BorrowedDate = null;
        }

        public void RestoreBorrow(string username, DateTime date)
        {
            BorrowedBy = username;
            BorrowedDate = date;
        }

        public string GetInfo()
        {
            return $"{Title} - {Author} ({Year}) | {this.Genre.ToGeorgian()} | {BookType}";
        }
    }
}