using Final_Project.Exceptions;
using Final_Project.Helpers;

namespace Final_Project.Models
{
    public class EBook : Book
    {
        public double FileSizeMb { get; private set; }

        public EBook(string title, string author, int year, Genre genre, double fileSizeMb)
            : base(title, author, year, genre)
        {
            if (!Validator.IsValidFileSize(fileSizeMb))
                throw new InvalidBookException("ფაილის ზომა უნდა იყოს 0-დან 5000 MB-მდე!");

            FileSizeMb = fileSizeMb;
        }

        public override string BookType => "ელექტრონული";

        public override string Details => $"{FileSizeMb} MB";
    }
}