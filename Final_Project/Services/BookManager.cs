using Final_Project.Data;
using Final_Project.Exceptions;
using Final_Project.Helpers;
using Final_Project.Models;

namespace Final_Project.Services
{
    public class BookManager
    {
        private const string FileName = "books.json";
        private const int MaxBorrowLimit = 3;

        private readonly List<Book> _books = new List<Book>();

        public Action<string>? OnBookAdded;

        public int Count => _books.Count;

        public int BorrowedCount => _books.Count(b => !b.IsAvailable);

        public async Task LoadAsync()
        {
            List<BookData> loaded = await JsonHelper.LoadListAsync<BookData>(FileName);

            _books.Clear();
            _books.AddRange(loaded.Select(d => d.ToBook()));
        }

        public async Task AddBookAsync(Book book)
        {
            await Task.Delay(300);

            AddToList(book);
            await SaveAsync();

            OnBookAdded?.Invoke(book.GetInfo());
        }

        public async Task AddRangeAsync(List<Book> books)
        {
            await Task.Delay(300);

            foreach (Book book in books)
            {
                AddToList(book);
            }

            await SaveAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync(Func<Book, object> sortBy)
        {
            await Task.Delay(300);

            if (_books.Count == 0)
                throw new EmptyLibraryException();

            return _books.OrderBy(sortBy).ToList();
        }

        public async Task<List<Book>> SearchByTitleAsync(string title)
        {
            await Task.Delay(300);

            CheckSearch(title);

            Func<Book, bool> filter = b => b.Title.Contains(title.Trim(), StringComparison.OrdinalIgnoreCase);

            List<Book> result = _books.Where(filter).ToList();

            if (result.Count == 0)
                throw new BookNotFoundException(title);

            return result;
        }

        public async Task<List<Book>> SearchByAuthorAsync(string author)
        {
            await Task.Delay(300);

            CheckSearch(author);

            Func<Book, bool> filter = b => b.Author.Contains(author.Trim(), StringComparison.OrdinalIgnoreCase);

            List<Book> result = _books.Where(filter).OrderBy(b => b.Year).ToList();

            if (result.Count == 0)
                throw new BookNotFoundException(author);

            return result;
        }

        public async Task<List<Book>> FilterByGenreAsync(Genre genre)
        {
            await Task.Delay(300);

            if (_books.Count == 0)
                throw new EmptyLibraryException();

            List<Book> result = _books.Where(b => b.Genre == genre).OrderBy(b => b.Title).ToList();

            if (result.Count == 0)
                throw new NoBooksInGenreException(genre.ToGeorgian());

            return result;
        }

        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            await Task.Delay(300);

            List<Book> result = _books.Where(b => b.IsAvailable).OrderBy(b => b.Title).ToList();

            if (result.Count == 0)
                throw new NoAvailableBooksException();

            return result;
        }

        public async Task<List<Book>> GetBorrowedByAsync(string username)
        {
            await Task.Delay(300);

            List<Book> result = _books.Where(b => b.BorrowedBy == username).OrderBy(b => b.Title).ToList();

            if (result.Count == 0)
                throw new NoBorrowedBooksException();

            return result;
        }

        public async Task BorrowBookAsync(Book book, string username)
        {
            await Task.Delay(300);

            int count = _books.Count(b => b.BorrowedBy == username);

            if (count >= MaxBorrowLimit)
                throw new BorrowLimitException(MaxBorrowLimit);

            book.Borrow(username);
            await SaveAsync();
        }

        public async Task ReturnBookAsync(Book book)
        {
            await Task.Delay(300);

            book.Return();
            await SaveAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            await Task.Delay(300);

            if (!book.IsAvailable)
                throw new DeleteBorrowedBookException(book.Title);

            _books.Remove(book);
            await SaveAsync();
        }

        public async Task<LibraryStats> GetStatsAsync()
        {
            await Task.Delay(300);

            if (_books.Count == 0)
                throw new EmptyLibraryException();

            return new LibraryStats
            {
                TotalBooks = _books.Count,
                BorrowedBooks = _books.Count(b => !b.IsAvailable),
                EBooks = _books.Count(b => b is EBook),
                OldestBook = _books.OrderBy(b => b.Year).First(),
                NewestBook = _books.OrderByDescending(b => b.Year).First(),
                ByGenre = _books.GroupBy(b => b.Genre).ToDictionary(g => g.Key, g => g.Count()),
                TopAuthors = _books
                    .GroupBy(b => b.Author)
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                    .ToList()
            };
        }

        private void CheckSearch(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new EmptySearchException();

            if (_books.Count == 0)
                throw new EmptyLibraryException();
        }

        private void AddToList(Book book)
        {
            bool exists = _books.Any(b => b.Title.ToLower() == book.Title.ToLower());

            if (exists)
                throw new DuplicateBookException(book.Title);

            _books.Add(book);
        }

        private async Task SaveAsync()
        {
            List<BookData> data = _books.Select(BookData.FromBook).ToList();

            await JsonHelper.SaveAsync(FileName, data);
        }
    }
}