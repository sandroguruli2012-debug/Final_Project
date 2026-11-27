using Final_Project.Models;

namespace Final_Project.Helpers
{
    public static class GenreExtensions
    {
        public static string ToGeorgian(this Genre genre)
        {
            return genre switch
            {
                Genre.Novel => "რომანი",
                Genre.Story => "მოთხრობა",
                Genre.Poem => "პოემა",
                Genre.Drama => "პიესა",
                Genre.Fantasy => "ფანტასტიკა",
                Genre.Detective => "დეტექტივი",
                Genre.History => "ისტორია",
                Genre.Science => "მეცნიერება",
                Genre.Children => "საბავშვო",
                Genre.Programming => "პროგრამირება",
                _ => "სხვა"
            };
        }
    }
}