using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
    public class ReviewerCommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string title { get; set; }
        public bool Credit { get; set; }
    }
}