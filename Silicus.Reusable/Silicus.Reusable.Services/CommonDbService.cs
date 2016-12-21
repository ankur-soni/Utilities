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
            var userDisplayName = _dataContextFactory.CreateCommonDBContext().Query<User>().Where(user => user.EmailAddress == email).FirstOrDefault().DisplayName;
            return userDisplayName;
        }

        public int FindUserIdFromEmail(string emailAddress)
        {
            var userId = _dataContextFactory.CreateCommonDBContext().Query<User>().Where(user => user.EmailAddress == emailAddress).FirstOrDefault().ID;
            return userId;
        }
    }
}
