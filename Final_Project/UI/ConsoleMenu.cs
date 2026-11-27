using Final_Project.Helpers;
using Final_Project.Models;
using Final_Project.Services;
using Spectre.Console;

namespace Final_Project.UI
{
    public class ConsoleMenu
    {
        private readonly BookManager _bookManager;
        private readonly UserManager _userManager;
        private readonly Dictionary<string, Func<Task>> _actions;
        private bool _running;

        public ConsoleMenu(BookManager bookManager, UserManager userManager)
        {
            _bookManager = bookManager;
            _userManager = userManager;

            _bookManager.OnBookAdded = info =>
                AnsiConsole.MarkupLine($"[green]✔ დაემატა:[/] {Markup.Escape(info)}");

            _actions = new Dictionary<string, Func<Task>>
            {
                ["1. წიგნის დამატება"] = AddBookAsync,
                ["2. ყველა წიგნის ნახვა"] = ShowAllBooksAsync,
                ["3. ძებნა სათაურით"] = SearchByTitleAsync,
                ["4. ძებნა ავტორით"] = SearchByAuthorAsync,
                ["5. ფილტრი ჟანრით"] = FilterByGenreAsync,
                ["6. წიგნის აღება"] = BorrowBookAsync,
                ["7. წიგნის დაბრუნება"] = ReturnBookAsync,
                ["8. ჩემი წიგნები"] = ShowMyBooksAsync,
                ["9. სტატისტიკა"] = ShowStatsAsync,
                ["10. წიგნის წაშლა"] = DeleteBookAsync,
                ["0. გასვლა"] = LogoutAsync
            };
        }

        private string CurrentUsername => _userManager.CurrentUser!.Username;

        public async Task RunAsync()
        {
            _running = true;

            while (_running)
            {
                AnsiConsole.Clear();
                ConsoleHelper.ShowTitle();
                ShowHeader();

                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]აირჩიე მოქმედება:[/]")
                        .PageSize(12)
                        .AddChoices(_actions.Keys));

                try
                {
                    await _actions[choice]();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowError(ex);
                }

                if (_running)
                    ConsoleHelper.Pause();
            }
        }

        private void ShowHeader()
        {
            User user = _userManager.CurrentUser!;

            string text =
                $"მომხმარებელი: [green]{Markup.Escape(user.Username)}[/]  |  " +
                $"რეგისტრაცია: [grey]{user.RegisteredAt:dd.MM.yyyy}[/]  |  " +
                $"წიგნები: [aqua]{_bookManager.Count}[/]  |  " +
                $"გაცემული: [yellow]{_bookManager.BorrowedCount}[/]";

            AnsiConsole.Write(new Panel(new Markup(text)).Border(BoxBorder.Rounded));
            AnsiConsole.WriteLine();
        }

        private Task LogoutAsync()
        {
            _userManager.Logout();
            _running = false;

            AnsiConsole.MarkupLine("[blue]გამოხვედი სისტემიდან.[/]");

            return Task.CompletedTask;
        }

        private async Task AddBookAsync()
        {
            string type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("წიგნის ტიპი:")
                    .AddChoices("ჩვეულებრივი", "ელექტრონული"));

            string title = AnsiConsole.Prompt(
                new TextPrompt<string>("სათაური:")
                    .Validate(t => Validator.IsValidText(t)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]სათაური ცარიელია![/]")));

            string author = AnsiConsole.Prompt(
                new TextPrompt<string>("ავტორი:")
                    .Validate(a => Validator.IsValidText(a)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]ავტორი ცარიელია![/]")));

            int year = AnsiConsole.Prompt(
                new TextPrompt<int>("გამოცემის წელი:")
                    .Validate(y => Validator.IsValidYear(y)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]წელი არასწორია![/]")));

            Genre genre = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("ჟანრი:")
                    .UseConverter(g => g.ToGeorgian())
                    .AddChoices(Enum.GetValues<Genre>()));

            Book book;

            if (type == "ელექტრონული")
            {
                double size = AnsiConsole.Prompt(
                    new TextPrompt<double>("ფაილის ზომა (MB):")
                        .Validate(s => Validator.IsValidFileSize(s)
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]ზომა არასწორია![/]")));

                book = new EBook(title, author, year, genre, size);
            }
            else
            {
                book = new PrintedBook(title, author, year, genre);
            }

            await AnsiConsole.Status().StartAsync("ინახება...", async ctx =>
            {
                await _bookManager.AddBookAsync(book);
            });
        }

        private async Task ShowAllBooksAsync()
        {
            Dictionary<string, Func<Book, object>> sortOptions = new Dictionary<string, Func<Book, object>>
            {
                ["სათაურით"] = b => b.Title,
                ["ავტორით"] = b => b.Author,
                ["წლით"] = b => b.Year,
                ["ჟანრით"] = b => b.Genre.ToGeorgian()
            };

            string sort = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("დალაგება:")
                    .AddChoices(sortOptions.Keys));

            List<Book> books = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                books = await _bookManager.GetAllBooksAsync(sortOptions[sort]);
            });

            ShowTable(books);
        }

        private async Task SearchByTitleAsync()
        {
            string title = AnsiConsole.Prompt(
                new TextPrompt<string>("შეიყვანე სათაური:").AllowEmpty());

            List<Book> found = new List<Book>();

            await AnsiConsole.Status().StartAsync("ვეძებ...", async ctx =>
            {
                found = await _bookManager.SearchByTitleAsync(title);
            });

            ShowTable(found);
        }

        private async Task SearchByAuthorAsync()
        {
            string author = AnsiConsole.Prompt(
                new TextPrompt<string>("შეიყვანე ავტორი:").AllowEmpty());

            List<Book> found = new List<Book>();

            await AnsiConsole.Status().StartAsync("ვეძებ...", async ctx =>
            {
                found = await _bookManager.SearchByAuthorAsync(author);
            });

            ShowTable(found);
        }

        private async Task FilterByGenreAsync()
        {
            Genre genre = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("აირჩიე ჟანრი:")
                    .UseConverter(g => g.ToGeorgian())
                    .AddChoices(Enum.GetValues<Genre>()));

            List<Book> found = new List<Book>();

            await AnsiConsole.Status().StartAsync("ვფილტრავ...", async ctx =>
            {
                found = await _bookManager.FilterByGenreAsync(genre);
            });

            ShowTable(found);
        }

        private async Task BorrowBookAsync()
        {
            List<Book> available = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                available = await _bookManager.GetAvailableBooksAsync();
            });

            Book selected = ChooseBook("აირჩიე წიგნი აღებისთვის:", available);

            await AnsiConsole.Status().StartAsync("მუშავდება...", async ctx =>
            {
                await _bookManager.BorrowBookAsync(selected, CurrentUsername);
            });

            AnsiConsole.MarkupLine($"[green]✔ წიგნი აიღე:[/] {Markup.Escape(selected.Title)}");
        }

        private async Task ReturnBookAsync()
        {
            List<Book> mine = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                mine = await _bookManager.GetBorrowedByAsync(CurrentUsername);
            });

            Book selected = ChooseBook("აირჩიე წიგნი დასაბრუნებლად:", mine);

            await AnsiConsole.Status().StartAsync("მუშავდება...", async ctx =>
            {
                await _bookManager.ReturnBookAsync(selected);
            });

            AnsiConsole.MarkupLine($"[green]✔ წიგნი დააბრუნე:[/] {Markup.Escape(selected.Title)}");
        }

        private async Task ShowMyBooksAsync()
        {
            List<Book> mine = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                mine = await _bookManager.GetBorrowedByAsync(CurrentUsername);
            });

            ShowTable(mine);
        }

        private async Task DeleteBookAsync()
        {
            List<Book> all = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                all = await _bookManager.GetAllBooksAsync(b => b.Title);
            });

            Book selected = ChooseBook("აირჩიე წიგნი წასაშლელად:", all);

            bool confirmed = AnsiConsole.Confirm(
                $"ნამდვილად გინდა წაშალო \"{Markup.Escape(selected.Title)}\"?", false);

            if (!confirmed)
            {
                AnsiConsole.MarkupLine("[yellow]წაშლა გაუქმდა.[/]");
                return;
            }

            await AnsiConsole.Status().StartAsync("იშლება...", async ctx =>
            {
                await _bookManager.DeleteBookAsync(selected);
            });

            AnsiConsole.MarkupLine("[green]✔ წიგნი წაიშალა.[/]");
        }

        private async Task ShowStatsAsync()
        {
            LibraryStats stats = new LibraryStats();

            await AnsiConsole.Status().StartAsync("ვითვლი...", async ctx =>
            {
                stats = await _bookManager.GetStatsAsync();
            });

            string topAuthors = string.Join(", ", stats.TopAuthors.Select(a => $"{a.Key} ({a.Value})"));

            Table table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("მაჩვენებელი");
            table.AddColumn("მნიშვნელობა");

            table.AddRow("სულ წიგნები", stats.TotalBooks.ToString());
            table.AddRow("გაცემული წიგნები", stats.BorrowedBooks.ToString());
            table.AddRow("ელექტრონული წიგნები", stats.EBooks.ToString());
            table.AddRow("ყველაზე ძველი", Markup.Escape(stats.OldestBook?.GetInfo() ?? "-"));
            table.AddRow("ყველაზე ახალი", Markup.Escape(stats.NewestBook?.GetInfo() ?? "-"));
            table.AddRow("ტოპ ავტორები", Markup.Escape(topAuthors));

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            Color[] colors =
            {
                Color.Aqua, Color.Green, Color.Yellow, Color.Red,
                Color.Blue, Color.Fuchsia, Color.Purple, Color.Lime
            };

            BarChart chart = new BarChart()
                .Width(60)
                .Label("[yellow]წიგნები ჟანრის მიხედვით[/]")
                .CenterLabel();

            int index = 0;

            foreach (KeyValuePair<Genre, int> item in stats.ByGenre.OrderByDescending(x => x.Value))
            {
                chart.AddItem(item.Key.ToGeorgian(), item.Value, colors[index % colors.Length]);
                index++;
            }

            AnsiConsole.Write(chart);
        }

        private Book ChooseBook(string title, List<Book> books)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title(title)
                    .PageSize(10)
                    .MoreChoicesText("[grey](ქვემოთ გადასასვლელად გამოიყენე ისრები)[/]")
                    .UseConverter(b => Markup.Escape($"{b.Title} - {b.Author}"))
                    .AddChoices(books));
        }

        private void ShowTable(List<Book> books)
        {
            Table table = new Table().Border(TableBorder.Rounded);

            table.AddColumn("#");
            table.AddColumn("ტიპი");
            table.AddColumn("სათაური");
            table.AddColumn("ავტორი");
            table.AddColumn("წელი");
            table.AddColumn("ჟანრი");
            table.AddColumn("დეტალი");
            table.AddColumn("სტატუსი");

            int number = 1;

            foreach (Book b in books)
            {
                string status = b.IsAvailable
                    ? "[green]თავისუფალია[/]"
                    : $"[red]გაცემულია: {Markup.Escape(b.BorrowedBy ?? "")}[/]";

                table.AddRow(
                    number.ToString(),
                    b.BookType,
                    Markup.Escape(b.Title),
                    Markup.Escape(b.Author),
                    b.Year.ToString(),
                    b.Genre.ToGeorgian(),
                    b.Details,
                    status);

                number++;
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"[grey]სულ: {books.Count}[/]");
        }
    }
}