using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ViewShortlistDetailsViewModel
    {
        public int NominationId { get; set; }
        public string UserName { get; set; }
        public string Manager { get; set; }
        public decimal TotalCredits { get; set; }
        public string NominationComment { get; set; }
        public string ProjectOrDepartment {get;set;}
        public List<ManagerComment> ManagerComments { get; set; }
        public bool IsShortlisted { get; set; }
        public bool IsWinner { get; set; }
        public List<Criteria> Criterias { get; set; }
        public List<List<ReviewerCommentViewModel>> ReviewerComments { get; set; }
        public bool IsLocked { get; set; }
        public string HrAdminsfeedback { get; set; }

        public string HrAdminName { get; set; }
    }
}