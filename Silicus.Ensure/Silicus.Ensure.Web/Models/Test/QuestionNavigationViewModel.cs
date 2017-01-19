using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class QuestionNavigationViewModel
    {
        public List<QuestionNavigationBasics> Practical { get; set; }
        public List<QuestionNavigationBasics> Objective { get; set; }
    }
}