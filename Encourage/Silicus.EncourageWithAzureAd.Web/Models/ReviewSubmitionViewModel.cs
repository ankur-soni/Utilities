﻿

using Silicus.Encourage.Models;
using System.Collections.Generic;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ReviewSubmitionViewModel
    {
        public int ReviewerId { get; set; }
        public string NomineeName { get; set; }
        public int NominationId { get; set; }
        public string ProjectOrDepartment { get; set; }
        public string Manager { get; set; }
        public string ManagerComment { get; set; }
        public List<ManagerComment> ManagerComments { get; set; }
        public List<Criteria> Criterias { get; set; }
        public IList<ReviewerCommentViewModel> Comments { get; set; }
        public int TotalCredit { get; set; }
        public bool? IsDrafted { get; set; }
        public bool IsLocked { get; set; }
        public ReviewSubmitionViewModel()
        {
            Comments = new List<ReviewerCommentViewModel>();   
        }
    }

}