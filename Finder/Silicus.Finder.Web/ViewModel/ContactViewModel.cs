using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Finder.Web.ViewModel
{
    public class ContactViewModel
    {
        public string Skype { get; set; }
        public string EmailAddress { get; set; } 
        public string PhoneNumber { get; set; }
        public Int64? MobileNumber { get; set; }
    }
}