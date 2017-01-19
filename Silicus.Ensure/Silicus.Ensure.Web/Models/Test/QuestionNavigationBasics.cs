using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class QuestionNavigationBasics
    {
        public List<int> Viewed { get; set; }
        public List<int> Answered { get; set; }
        public List<int> QuestionIds { get; set; }
    }
}