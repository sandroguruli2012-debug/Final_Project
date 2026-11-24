namespace Final_Project.Exceptions
{
    public class EmptySearchException : Exception
    {
        public EmptySearchException() : base("საძიებო სიტყვა არ შეიძლება იყოს ცარიელი!")
        {
        }
    }
}