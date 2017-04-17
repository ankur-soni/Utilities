using Silicus.Ensure.Models.Constants;
using System;

namespace Silicus.Ensure.Models.DataObjects
{
    public class QuestionStatusDetails
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionStatus Status { get; set; }
        public string Comment { get; set; }
        public DateTime ChangedDate { get; set; }
        public int ChangedBy { get; set; }

        public virtual Question Question { get; set; }
    }
}
