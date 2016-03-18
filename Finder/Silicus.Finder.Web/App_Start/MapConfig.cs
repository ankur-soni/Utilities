using AutoMapper;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Finder.Web
{
    public static class MapConfig
    {
        public static void RegisterMap()
        {
            Mapper.CreateMap<Employee, EmployeeViewModel>();
        }
    }
}