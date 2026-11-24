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
                new PrintedBook("ვეფხისტყაოსანი", "შოთა რუსთაველი", 1200),
                new PrintedBook("დათა თუთაშხია", "ჭაბუა ამირეჯიბი", 1973),
                new PrintedBook("გორა მბორგალი", "ჭაბუა ამირეჯიბი", 1987),
                new EBook("ჯაყოს ხიზნები", "მიხეილ ჯავახიშვილი", 1925, 3.2),
                new PrintedBook("კვაჭი კვაჭანტირაძე", "მიხეილ ჯავახიშვილი", 1924),
                new PrintedBook("არსენა მარაბდელი", "მიხეილ ჯავახიშვილი", 1933),
                new PrintedBook("კაცია-ადამიანი?!", "ილია ჭავჭავაძე", 1863),
                new PrintedBook("ოთარაანთ ქვრივი", "ილია ჭავჭავაძე", 1887),
                new PrintedBook("განდეგილი", "ილია ჭავჭავაძე", 1883),
                new PrintedBook("გლახის ნაამბობი", "ილია ჭავჭავაძე", 1863),
                new PrintedBook("ალუდა ქეთელაური", "ვაჟა-ფშაველა", 1888),
                new PrintedBook("სტუმარ-მასპინძელი", "ვაჟა-ფშაველა", 1893),
                new PrintedBook("გველისმჭამელი", "ვაჟა-ფშაველა", 1901),
                new PrintedBook("ელისო", "ალექსანდრე ყაზბეგი", 1882),
                new EBook("დიდოსტატის მარჯვენა", "კონსტანტინე გამსახურდია", 1939, 5.4),
                new PrintedBook("დიონისოს ღიმილი", "კონსტანტინე გამსახურდია", 1925),
                new PrintedBook("მთვარის მოტაცება", "კონსტანტინე გამსახურდია", 1935),
                new PrintedBook("მე, ბებია, ილიკო და ილარიონი", "ნოდარ დუმბაძე", 1960),
                new PrintedBook("მე ვხედავ მზეს", "ნოდარ დუმბაძე", 1962),
                new PrintedBook("კუკარაჩა", "ნოდარ დუმბაძე", 1973),
                new PrintedBook("მარადისობის კანონი", "ნოდარ დუმბაძე", 1978),
                new EBook("გველისპირა ქალწული", "გრიგოლ რობაქიძე", 1928, 2.8),
                new PrintedBook("სამანიშვილის დედინაცვალი", "დავით კლდიაშვილი", 1896),
                new PrintedBook("გვადი ბიგვა", "ლეო ქიაჩელი", 1925),
                new EBook("რკინის თეატრი", "ოთარ ჭილაძე", 1975, 4.6),
                new EBook("კაცი, რომელიც ჩრდილს მისდევდა", "ოთარ ჭილაძე", 1972, 3.9),
                new PrintedBook("გამზრდელი", "აკაკი წერეთელი", 1886)
            };

            await manager.AddRangeAsync(books);
        }
    }
}