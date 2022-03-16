using OpenDIAPIServer.models;
using System.Collections.Generic;

namespace OpenDIAPIServer.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}
