
using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
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

        public ReviewSubmitionViewModel()
        {
            Comments = new List<ReviewerCommentViewModel>();   
        }
    }

}