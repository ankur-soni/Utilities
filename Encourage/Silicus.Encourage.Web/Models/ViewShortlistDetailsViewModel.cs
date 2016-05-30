using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
    public class ViewShortlistDetailsViewModel
    {
        public string userName { get; set; }
        public string Manager { get; set; }
        public int totalCredits { get; set; }
        public string nominationComments { get; set; }
        public string projectOrDepartment {get;set;}
        public List<List<ReviewerCommentViewModel>> reviewerComments { get; set; }
    }
}