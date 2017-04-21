using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.ReviewQuestion
{
    public class ReviewQuestionBusinessModel
    {
        public Question QuestionDetails { get; set; }
        public int? NextQuestionId { get; set; }
        public int TechnologyId { get; set; }
    }
}
