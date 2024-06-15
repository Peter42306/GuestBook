namespace GuestBook.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, string salt);
    }
}
