using Final_Project.Exceptions;
using Spectre.Console;

namespace Final_Project.UI
{
    public static class ConsoleHelper
    {
        public static void ShowTitle()
        {
            AnsiConsole.Write(new FigletText("Book Manager").Color(Color.Aqua));
            AnsiConsole.Write(new Rule("[grey]წიგნების მართვის სისტემა[/]"));
            AnsiConsole.WriteLine();
        }

        public static void ShowError(Exception ex)
        {
            bool isWarning = ex is BookNotFoundException
                or EmptyLibraryException
                or NoBooksInGenreException
                or NoBorrowedBooksException
                or NoAvailableBooksException;

            string color = isWarning ? "yellow" : "red";

            AnsiConsole.MarkupLine($"[{color}]✖ {Markup.Escape(ex.Message)}[/]");
        }

        public static ValidationResult Check(string? error)
        {
            if (error == null)
                return ValidationResult.Success();

            return ValidationResult.Error($"[red]{error}[/]");
        }

        public static void Pause()
        {
            AnsiConsole.MarkupLine("\n[grey]დააჭირე ნებისმიერ ღილაკს...[/]");
            Console.ReadKey(true);
        }
    }
}