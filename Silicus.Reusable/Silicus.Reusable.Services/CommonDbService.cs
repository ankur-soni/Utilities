using Silicus.FrameworxProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Models.DataObjects;

namespace Silicus.FrameworxProject.Services
{
    public class CommonDbService : ICommonDbService
    {
        private readonly Silicus.UtilityContainer.Entities.IDataContextFactory _dataContextFactory;

        public CommonDbService(Silicus.UtilityContainer.Entities.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
        }
        public UtilityContainer.Entities.ICommonDataBaseContext GetCommonDataBaseContext()
        {
            return _dataContextFactory.CreateCommonDBContext();
        }

        public string FindDisplayNameFromEmail(string email)
        {
            var user = _dataContextFactory.CreateCommonDBContext().Query<User>().Where(usr => usr.EmailAddress == email).ToList();
            return user != null && user.Count > 0 ? user.FirstOrDefault().DisplayName : null;
        }

        public int? FindUserIdFromEmail(string emailAddress)
        {
            var user = _dataContextFactory.CreateCommonDBContext().Query<User>().Where(usr => usr.EmailAddress == emailAddress).ToList();
            return user != null && user.Count > 0 ? user.FirstOrDefault().ID : (int?)null;
        }

        public User GetUser(int userId)
        {
            var user = _dataContextFactory.CreateCommonDBContext().Query<User>().Where(usr => usr.ID == userId).ToList();
            return user != null && user.Count > 0 ? user.FirstOrDefault() : null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dataContextFactory.CreateCommonDBContext().Query<User>().ToList();
        }

    }
}
