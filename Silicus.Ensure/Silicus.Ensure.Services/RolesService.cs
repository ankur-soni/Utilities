﻿using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class RolesService : IRolesService
    {
        private readonly IDataContext _context;

        public RolesService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<Role> GetRoleDetails()
        {
            var RoleList = _context.Query<Role>().ToList();
            return RoleList;

        }

        public int Add(Role Role)
        {
            _context.Add(Role);
            return Role.RoleId;
        }

        public void Update(Role Role)
        {
            if (Role.RoleName != null && Role.Description != null)
            {
                _context.Update(Role);
            }
        }

        public void Delete(Role Role)
        {
            if (Role.RoleName != null && Role.Description != null)
            {
                _context.Delete(Role);
            }
        }
    }
}

