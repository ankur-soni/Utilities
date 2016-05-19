using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
  
    public class ReviewNominationViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string NominationTime { get; set; }
        public string AwardName { get; set; }
        public string Intials { get; set; }
        public int ManagerId { get; set; }
    }
}