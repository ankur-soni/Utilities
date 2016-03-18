using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class UserListViewModel
    {
        public string UserName { get; set; }

        public string UserDisplayName { get; set; }

        public int ProjectCount { get; set; }
    }
}