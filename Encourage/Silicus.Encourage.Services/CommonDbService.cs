using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
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
            var userDisplayName = string.Empty;
            var user = _dataContextFactory.CreateCommonDBContext().Query<User>().FirstOrDefault(u => u.EmailAddress == email);
            if (user != null)
            {
                userDisplayName = user.DisplayName;
            }

            return userDisplayName;
        }
    }
}
