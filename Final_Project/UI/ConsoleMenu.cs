
using Final_Project.Exceptions;
using Final_Project.Helpers;
using Final_Project.Models;
using Final_Project.Services;
using Spectre.Console;

namespace Final_Project.UI
{
    public class ConsoleMenu
    {
        private readonly BookManager _manager;
        private readonly UserManager _userManager;

        public ConsoleMenu(BookManager manager, UserManager userManager)
        {
            _manager = manager;
            _userManager = userManager;

            _manager.OnBookAdded = info =>
                AnsiConsole.MarkupLine($"[green]✔ დაემატა:[/] {Markup.Escape(info)}");
        }

        public async Task RunAsync()
        {
            bool running = true;

            while (running)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[grey]წიგნების რაოდენობა: {_manager.Count}[/]\n");

                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]აირჩიე მოქმედება:[/]")
                        .AddChoices(
                            "1. წიგნის დამატება",
                            "2. ყველა წიგნის ნახვა",
                            "3. წიგნის ძებნა",
                            "4. გასვლა"));

                try
                {
                    switch (choice[0])
                    {
                        case '1':
                            await AddBookAsync();
                            break;

                        case '2':
                            await ShowAllBooksAsync();
                            break;

                        case '3':
                            await SearchBookAsync();
                            break;

                        case '4':
                            running = false;
                            break;
                    }
                }
                catch (BookNotFoundException ex)
                {
                    AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(ex.Message)}[/]");
                }
                catch (EmptyLibraryException ex)
                {
                    AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(ex.Message)}[/]");
                }
                catch (EmptySearchException ex)
                {
                    AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                }
                catch (DuplicateBookException ex)
                {
                    AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                }
                catch (InvalidBookException ex)
                {
                    AnsiConsole.MarkupLine($"[red]შეცდომა: {Markup.Escape(ex.Message)}[/]");
                }

                if (running)
                {
                    AnsiConsole.MarkupLine("\n[grey]დააჭირე ნებისმიერ ღილაკს...[/]");
                    Console.ReadKey(true);
                }
            }

            AnsiConsole.MarkupLine("[blue]ნახვამდის![/]");
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

            Book book;

            if (type == "ელექტრონული")
            {
                double size = AnsiConsole.Prompt(
                    new TextPrompt<double>("ფაილის ზომა (MB):")
                        .Validate(s => Validator.IsValidFileSize(s)
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]ზომა არასწორია![/]")));

                book = new EBook(title, author, year, size);
            }
            else
            {
                book = new PrintedBook(title, author, year);
            }

            await AnsiConsole.Status().StartAsync("ინახება...", async ctx =>
            {
                await _manager.AddBookAsync(book);
            });
        }

        private async Task ShowAllBooksAsync()
        {
            List<Book> books = new List<Book>();

            await AnsiConsole.Status().StartAsync("იტვირთება...", async ctx =>
            {
                books = await _manager.GetAllBooksAsync();
            });

            ShowTable(books);
        }

        private async Task SearchBookAsync()
        {
            string title = AnsiConsole.Prompt(
                new TextPrompt<string>("შეიყვანე სათაური:")
                    .AllowEmpty());

            List<Book> found = new List<Book>();

            await AnsiConsole.Status().StartAsync("ვეძებ...", async ctx =>
            {
                found = await _manager.SearchByTitleAsync(title);
            });

            ShowTable(found);
        }

        private void ShowTable(List<Book> books)
        {
            Table table = new Table().Border(TableBorder.Rounded);

            table.AddColumn("#");
            table.AddColumn("ტიპი");
            table.AddColumn("სათაური");
            table.AddColumn("ავტორი");
            table.AddColumn("წელი");
            table.AddColumn("დეტალი");

            int number = 1;

            foreach (Book b in books)
            {
                table.AddRow(
                    number.ToString(),
                    b.BookType,
                    Markup.Escape(b.Title),
                    Markup.Escape(b.Author),
                    b.Year.ToString(),
                    b.Details);

                number++;
            }

            AnsiConsole.Write(table);
        }
    }
}
