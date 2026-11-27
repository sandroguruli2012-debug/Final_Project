namespace Final_Project.Exceptions
{
    public class BorrowLimitException : Exception
    {
        public BorrowLimitException(int limit)
            : base($"ერთ მომხმარებელს შეუძლია მაქსიმუმ {limit} წიგნის აღება!")
        {
        }
    }
}