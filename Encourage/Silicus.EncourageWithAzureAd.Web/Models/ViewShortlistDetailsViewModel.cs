using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ViewShortlistDetailsViewModel
    {
        public int nominationId { get; set; }
        public string userName { get; set; }
        public string Manager { get; set; }
        public int totalCredits { get; set; }
        public string nominationComment { get; set; }
        public string projectOrDepartment {get;set;}
        public List<ManagerComment> ManagerComments { get; set; }
        public bool IsShortlisted { get; set; }
        public bool IsWinner { get; set; }
        public List<Criteria> Criterias { get; set; }
        public List<List<ReviewerCommentViewModel>> reviewerComments { get; set; }
    }
}