using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ReviewerCommentViewModel
    {
        public int Id { get; set; }
        public int CriteriaId { get; set; }
        public int ReviewerId { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public int Credit { get; set; }
        public string ReviewerName { get; set; }
        public int Weightage { get; set; }
    }
}