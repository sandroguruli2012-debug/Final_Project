namespace Final_Project.Exceptions
{
    public class WrongSecretWordException : Exception
    {
        public WrongSecretWordException() : base("საიდუმლო სიტყვა არასწორია!")
        {
        }
    }
}