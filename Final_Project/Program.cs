using Final_Project.Data;
using Final_Project.Services;
using Final_Project.UI;

// ქართული ასოებისთვის
Console.OutputEncoding = System.Text.Encoding.UTF8;

BookManager manager = new BookManager();

// წინასწარ ვამატებთ ქართულ წიგნებს
await BookSeeder.SeedAsync(manager);

ConsoleMenu menu = new ConsoleMenu(manager);
await menu.RunAsync();