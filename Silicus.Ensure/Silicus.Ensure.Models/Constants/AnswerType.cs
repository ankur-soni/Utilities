using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.Constants
{
    public enum AnswerType
    {
        [Description("Single Choice")]
        Single_Choice = 1,
        [Description("Multiple Choice")]
        Multiple_Choice = 2
    }
}
