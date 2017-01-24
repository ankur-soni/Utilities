using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class CustomdateViewmodel
    {
        public int Id { get; set; }
        public bool IsApplicable { get; set; }
        public int MonthToSubtract { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
        public  List<Award> Awards { get; set; }

    }
}