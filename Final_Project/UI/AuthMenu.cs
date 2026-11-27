
using Final_Project.Exceptions;
using Final_Project.Helpers;
using Final_Project.Services;
using Spectre.Console;

namespace Final_Project.UI
{
    public class AuthMenu
    {
        private const int MaxAttempts = 3;

        private readonly UserManager _userManager;

        public AuthMenu(UserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> RunAsync()
        {
            while (true)
            {
                AnsiConsole.Clear();
                ConsoleHelper.ShowTitle();

                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]კეთილი იყოს შენი მობრძანება:[/]")
                        .AddChoices(
                            "1. შესვლა",
                            "2. რეგისტრაცია",
                            "3. პაროლის აღდგენა",
                            "0. გასვლა"));

                try
                {
                    switch (choice[0])
                    {
                        case '1':
                            if (await LoginAsync())
                                return true;
                            break;

                        case '2':
                            await RegisterAsync();
                            break;

                        case '3':
                            await ForgotPasswordAsync();
                            break;

                        case '0':
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowError(ex);
                }

                ConsoleHelper.Pause();
            }
        }

        private async Task<bool> LoginAsync()
        {
            int attempts = 0;

            while (attempts < MaxAttempts)
            {
                string username = AnsiConsole.Prompt(
                    new TextPrompt<string>("მომხმარებლის სახელი:"));

                string password = AnsiConsole.Prompt(
                    new TextPrompt<string>("პაროლი:").Secret());

                try
                {
                    await AnsiConsole.Status().StartAsync(
                        "მოწმდება...", async ctx =>
                        {
                            await _userManager.LoginAsync(username, password);
                        });

                    AnsiConsole.MarkupLine(
                        "[green]✔ წარმატებით შეხვედი![/]");

                    await Task.Delay(800);

                    return true;
                }
                catch (InvalidCredentialsException ex)
                {
                    attempts++;

                    AnsiConsole.MarkupLine(
                        $"[red]✖ {Markup.Escape(ex.Message)} დარჩენილია მცდელობა: {MaxAttempts - attempts}[/]");
                }
            }

            throw new TooManyAttemptsException();
        }

        private async Task RegisterAsync()
        {
            string username = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "სახელი (4-15 სიმბოლო, ასოები/ციფრები/_):")
                    .Validate(u => ConsoleHelper.Check(
                        Validator.GetUsernameError(u))));

            string password = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "პაროლი (მინ. 6 სიმბოლო, ასო და ციფრი):")
                    .Secret()
                    .Validate(p => ConsoleHelper.Check(
                        Validator.GetPasswordError(username, p))));

            string confirm = AnsiConsole.Prompt(
                new TextPrompt<string>("გაიმეორე პაროლი:").Secret());

            if (password != confirm)
                throw new PasswordMismatchException();

            string secretWord = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "საიდუმლო სიტყვა (პაროლის აღსადგენად):")
                    .Validate(s => ConsoleHelper.Check(
                        Validator.GetSecretWordError(s))));

            await AnsiConsole.Status().StartAsync(
                "მომხმარებელი იქმნება...", async ctx =>
                {
                    await _userManager.RegisterAsync(
                        username, password, secretWord);
                });

            AnsiConsole.MarkupLine(
                "[green]✔ რეგისტრაცია წარმატებით დასრულდა![/]");
        }

        private async Task ForgotPasswordAsync()
        {
            string username = AnsiConsole.Prompt(
                new TextPrompt<string>("მომხმარებლის სახელი:"));

            string secretWord = AnsiConsole.Prompt(
                new TextPrompt<string>("საიდუმლო სიტყვა:").Secret());

            await AnsiConsole.Status().StartAsync(
                "მოწმდება...", async ctx =>
                {
                    await _userManager.VerifySecretWordAsync(
                        username, secretWord);
                });

            AnsiConsole.MarkupLine(
                "[green]✔ საიდუმლო სიტყვა სწორია![/]");

            string newPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("ახალი პაროლი:")
                    .Secret()
                    .Validate(p => ConsoleHelper.Check(
                        Validator.GetPasswordError(username, p))));

            string confirm = AnsiConsole.Prompt(
                new TextPrompt<string>("გაიმეორე ახალი პაროლი:").Secret());

            if (newPassword != confirm)
                throw new PasswordMismatchException();

            await AnsiConsole.Status().StartAsync(
                "პაროლი იცვლება...", async ctx =>
                {
                    await _userManager.ResetPasswordAsync(
                        username, secretWord, newPassword);
                });

            AnsiConsole.MarkupLine(
                "[green]✔ პაროლი წარმატებით შეიცვალა![/]");
        }
    }
}
