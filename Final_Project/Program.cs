using Final_Project.Data;
using Final_Project.Services;
using Final_Project.UI;
using Spectre.Console;

Console.OutputEncoding = System.Text.Encoding.UTF8;

BookManager manager = new BookManager();
UserManager userManager = new UserManager();

ConsoleMenu menu = new ConsoleMenu(manager, userManager);

bool loggedIn = false;

while (!loggedIn)
{
    AnsiConsole.Clear();

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[yellow]აირჩიე მოქმედება:[/]")
            .AddChoices(
                "1. რეგისტრაცია",
                "2. ლოგინი",
                "3. გასვლა"));

    switch (choice[0])
    {
        case '1':
            string newUsername = AnsiConsole.Ask<string>("მომხმარებლის სახელი:");
            string newPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("პაროლი:").Secret());

            try
            {
                await userManager.RegisterAsync(newUsername, newPassword);
                AnsiConsole.MarkupLine("[green]რეგისტრაცია წარმატებულია![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
            }

            break;

        case '2':
            string username = AnsiConsole.Ask<string>("მომხმარებლის სახელი:");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("პაროლი:").Secret());

            loggedIn = await userManager.LoginAsync(username, password);

            if (loggedIn)
            {
                AnsiConsole.MarkupLine("[green]წარმატებით შეხვედით სისტემაში![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]მომხმარებელი ან პაროლი არასწორია![/]");
            }

            break;

        case '3':
            return;
    }

    if (!loggedIn)
    {
        AnsiConsole.MarkupLine("\n[grey]დააჭირე ნებისმიერ ღილაკს...[/]");
        Console.ReadKey(true);
    }
}

await BookSeeder.SeedAsync(manager);

await menu.RunAsync();