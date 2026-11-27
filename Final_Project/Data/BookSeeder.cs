using Final_Project.Models;
using Final_Project.Services;

namespace Final_Project.Data
{
    public static class BookSeeder
    {
        public static async Task SeedAsync(BookManager manager)
        {
            List<Book> books = new List<Book>
            {
                new PrintedBook("ვეფხისტყაოსანი", "შოთა რუსთაველი", 1200, Genre.Poem),
                new PrintedBook("დათა თუთაშხია", "ჭაბუა ამირეჯიბი", 1973, Genre.Novel),
                new PrintedBook("გორა მბორგალი", "ჭაბუა ამირეჯიბი", 1987, Genre.Novel),
                new EBook("ჯაყოს ხიზნები", "მიხეილ ჯავახიშვილი", 1925, Genre.Novel, 3.2),
                new PrintedBook("კვაჭი კვაჭანტირაძე", "მიხეილ ჯავახიშვილი", 1924, Genre.Novel),
                new PrintedBook("არსენა მარაბდელი", "მიხეილ ჯავახიშვილი", 1933, Genre.Novel),
                new PrintedBook("კაცია-ადამიანი?!", "ილია ჭავჭავაძე", 1863, Genre.Story),
                new PrintedBook("ოთარაანთ ქვრივი", "ილია ჭავჭავაძე", 1887, Genre.Story),
                new PrintedBook("განდეგილი", "ილია ჭავჭავაძე", 1883, Genre.Poem),
                new PrintedBook("გლახის ნაამბობი", "ილია ჭავჭავაძე", 1863, Genre.Story),
                new PrintedBook("ალუდა ქეთელაური", "ვაჟა-ფშაველა", 1888, Genre.Poem),
                new PrintedBook("სტუმარ-მასპინძელი", "ვაჟა-ფშაველა", 1893, Genre.Poem),
                new PrintedBook("გველისმჭამელი", "ვაჟა-ფშაველა", 1901, Genre.Poem),
                new PrintedBook("ელისო", "ალექსანდრე ყაზბეგი", 1882, Genre.Story),
                new EBook("დიდოსტატის მარჯვენა", "კონსტანტინე გამსახურდია", 1939, Genre.History, 5.4),
                new PrintedBook("დიონისოს ღიმილი", "კონსტანტინე გამსახურდია", 1925, Genre.Novel),
                new PrintedBook("მთვარის მოტაცება", "კონსტანტინე გამსახურდია", 1935, Genre.Novel),
                new PrintedBook("მე, ბებია, ილიკო და ილარიონი", "ნოდარ დუმბაძე", 1960, Genre.Novel),
                new PrintedBook("მე ვხედავ მზეს", "ნოდარ დუმბაძე", 1962, Genre.Novel),
                new PrintedBook("კუკარაჩა", "ნოდარ დუმბაძე", 1973, Genre.Novel),
                new PrintedBook("მარადისობის კანონი", "ნოდარ დუმბაძე", 1978, Genre.Novel),
                new EBook("გველისპირა ქალწული", "გრიგოლ რობაქიძე", 1928, Genre.Novel, 2.8),
                new PrintedBook("სამანიშვილის დედინაცვალი", "დავით კლდიაშვილი", 1896, Genre.Drama),
                new PrintedBook("გვადი ბიგვა", "ლეო ქიაჩელი", 1925, Genre.Novel),
                new EBook("რკინის თეატრი", "ოთარ ჭილაძე", 1975, Genre.Novel, 4.6),
                new EBook("კაცი, რომელიც ჩრდილს მისდევდა", "ოთარ ჭილაძე", 1972, Genre.Novel, 3.9),
                new PrintedBook("გამზრდელი", "აკაკი წერეთელი", 1886, Genre.Poem),
                new EBook("Clean Code", "Robert Martin", 2008, Genre.Programming, 4.5),
                new PrintedBook("ჰარი პოტერი და ფილოსოფიური ქვა", "J.K. Rowling", 1997, Genre.Fantasy),
                new PrintedBook("შერლოკ ჰოლმსის თავგადასავალი", "Arthur Conan Doyle", 1892, Genre.Detective),
                new PrintedBook("პატარა უფლისწული", "Antoine de Saint-Exupéry", 1943, Genre.Children)
            };

            await manager.AddRangeAsync(books);
        }
    }
}