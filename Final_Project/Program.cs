using Final_Project.Data;
using Final_Project.Services;
using Final_Project.UI;
using Spectre.Console;

Console.OutputEncoding = System.Text.Encoding.UTF8;

UserManager userManager = new UserManager();
BookManager bookManager = new BookManager();

try
{
    await userManager.LoadAsync();
    await bookManager.LoadAsync();

    if (bookManager.Count == 0)
        await BookSeeder.SeedAsync(bookManager);
}
catch (Exception ex)
{
    ConsoleHelper.ShowError(ex);
    ConsoleHelper.Pause();
    return;
}

AuthMenu authMenu = new AuthMenu(userManager);
ConsoleMenu mainMenu = new ConsoleMenu(bookManager, userManager);

while (true)
{
    bool loggedIn = await authMenu.RunAsync();

    if (!loggedIn)
        break;

    await mainMenu.RunAsync();
}

AnsiConsole.MarkupLine("[blue]ნახვამდის![/]");