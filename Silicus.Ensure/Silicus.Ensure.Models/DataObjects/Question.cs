using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Question
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionCategory { get; set; }
        public int QuestionType { get; set; }
        public int Marks { get; set; }
    }
}
