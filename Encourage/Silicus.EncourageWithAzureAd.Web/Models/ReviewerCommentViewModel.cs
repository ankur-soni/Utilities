using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ReviewerCommentViewModel
    {
        public int Id { get; set; }
        public int CriteriaID { get; set; }
        public string Comment { get; set; }
        public string title { get; set; }
        public bool Credit { get; set; }
        public string ReviewerName { get; set; }
        public int Weightage { get; set; }
    }
}