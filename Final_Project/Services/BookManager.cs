using Final_Project.Exceptions;
using Final_Project.Models;

namespace Final_Project.Services
{
    public class BookManager
    {        private readonly List<Book> _books = new List<Book>();

        public Action<string>? OnBookAdded;

        public int Count => _books.Count;

        public async Task AddBookAsync(Book book)
        {
            await Task.Delay(300);

            AddToList(book);
            OnBookAdded?.Invoke(book.GetInfo());
        }

        public async Task AddRangeAsync(List<Book> books)
        {
            await Task.Delay(300);

            foreach (Book book in books)
            {
                AddToList(book);
            }
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            await Task.Delay(300);

            if (_books.Count == 0)
                throw new EmptyLibraryException();
            return _books.OrderBy(b => b.Title).ToList();
        }

        public async Task<List<Book>> SearchByTitleAsync(string title)
        {
            await Task.Delay(300);

            if (string.IsNullOrWhiteSpace(title))
                throw new EmptySearchException();

            if (_books.Count == 0)
                throw new EmptyLibraryException();

            Func<Book, bool> filter = b => b.Title.Contains(title.Trim(), StringComparison.OrdinalIgnoreCase);
            List<Book> result = _books.Where(filter).ToList();

            if (result.Count == 0)
                throw new BookNotFoundException(title);

            return result;
        }

        private void AddToList(Book book)
        {
            bool exists = _books.Any(b => b.Title.ToLower() == book.Title.ToLower());

            if (exists)
                throw new DuplicateBookException(book.Title);

            _books.Add(book);
        }
    }
}