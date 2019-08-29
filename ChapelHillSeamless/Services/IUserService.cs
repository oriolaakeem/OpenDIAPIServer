using ChapelHill.models;
using System.Collections.Generic;

namespace ChapelHill.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}
