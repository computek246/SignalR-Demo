using System.Collections.Generic;
using SignalRDemo.Server.Entities;
using SignalRDemo.Server.Models;

namespace SignalRDemo.Server.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
