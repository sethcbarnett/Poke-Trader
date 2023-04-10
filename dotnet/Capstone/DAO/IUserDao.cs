using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserDao
    {
        User GetUser(string username);
        User AddUser(string username, string password, string role, string email, string streetAddress, string city, string stateAbbreviation, int zipCode);
    }
}
