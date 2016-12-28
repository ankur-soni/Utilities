using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ProcessesToLockOrUnLockViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}