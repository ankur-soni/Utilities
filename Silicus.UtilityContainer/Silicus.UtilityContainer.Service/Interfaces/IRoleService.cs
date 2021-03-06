﻿using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Services.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetAllRoles();
        Role GetRoleByRoleName(string RoleName);
        string GetRoleName(int roleId);
    }
}
