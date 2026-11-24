namespace Final_Project.Exceptions
{
    public class EmptyLibraryException : Exception
    {
        public EmptyLibraryException() : base("ბიბლიოთეკა ცარიელია.")
        {
        }
    }
}