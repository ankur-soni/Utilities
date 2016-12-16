using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class CriteriaCommentViewModel
    {
        public int Id { get; set; }
        public int Weightage { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
    }
}