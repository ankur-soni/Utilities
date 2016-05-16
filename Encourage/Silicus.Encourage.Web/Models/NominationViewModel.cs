using Silicus.Encourage.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
    public class NominationViewModel
    {
        public int AwardId { get; set; }
        public int ManagerId { get; set; }
        public int UserId { get; set; }
        public int ProjectID { get; set; }
        public string SelectResourcesBy { get; set; }  //from project or from department
        public IList<CriteriaCommentViewModel> Comments { get; set; }

        public NominationViewModel()
        {
            Comments = new List<CriteriaCommentViewModel>();   
        }
    
    }
}