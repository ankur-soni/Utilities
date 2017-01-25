using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Test
{
    public class QuestionNavigationBasics
    {
        public int QuestionId { get; set; }
        public bool IsViewedOnly { get; set; }
        public bool IsAnswered { get; set; }
    }
}
