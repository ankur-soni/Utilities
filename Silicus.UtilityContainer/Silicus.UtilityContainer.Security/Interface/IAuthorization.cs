﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Security.Interface
{
    public interface IAuthorization
    {
        List<string> GetRoleForUtility(string email, string utiltyName);
        List<string> GetNameOfContributors(int utilityId);
    }
}
