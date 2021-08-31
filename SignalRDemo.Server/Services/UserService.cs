using System.Collections.Generic;
using System.Linq;
using SignalRDemo.Server.Entities;
using SignalRDemo.Server.Models;

namespace SignalRDemo.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService<User> _tokenService;

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<User> _users = new()
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };


        public UserService(ITokenService<User> tokenService)
        {
            _tokenService = tokenService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = _tokenService.GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }


    }
}