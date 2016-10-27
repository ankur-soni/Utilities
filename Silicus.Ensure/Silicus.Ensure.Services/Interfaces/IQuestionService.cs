﻿using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services.Interfaces
{
    public interface IQuestionService
    {
        IEnumerable<Question> GetQuestion();

        int Add(Question Question);
            
    }
}
