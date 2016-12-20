using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ConsolidatedNominationsViewModel
    {
        public List<Criteria> Criterias { get; set; }

        public List<Reviewer> Reviewers { get; set; }
        public List<SubmittedNomination> Nominations { get; set; }
    }

    public class SubmittedNomination
    {
        public int NominationId { get; set; }
        public string UserName { get; set; } 
        public List<ManagerComment> ManagerComments { get; set; }           
        public List<ReviewerCommentViewModel> ReviewerComments { get; set; }               
    }
}