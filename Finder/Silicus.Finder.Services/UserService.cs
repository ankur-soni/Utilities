using System.Collections.Generic;
using System.Linq;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;

namespace Silicus.Finder.Services
{
    public class UserService : IUserService
    {
        //private readonly IDataContext _context;

        //public UserService(IDataContextFactory dataContextFactory)
        //{
        //    _context = dataContextFactory.Create(ConnectionType.Ip);
        //}

        //public IEnumerable<User> GetUserDetails()
        //{
        //    return _context.Query<User>();
        //}

        //public int Add(User User)
        //{
        //    _context.Add(User);
        //    return User.UserId;
        //}

        //public void Update(User User)
        //{
        //    if (User.FirstName != null && User.Address != null && User.LastName != null && User.Role != null)
        //    {
        //        _context.Update(User);
        //    }
        //}

        //public void Delete(User User)
        //{
        //    if (User.FirstName != null && User.Address != null && User.LastName != null && User.Role != null)
        //    {
        //        _context.Delete(User);
        //    }
        //}
    }
}

