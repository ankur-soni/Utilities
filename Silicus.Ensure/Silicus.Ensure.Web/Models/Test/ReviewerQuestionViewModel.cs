﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class ReviewerQuestionViewModel : TestDetailsViewModel
    {

        public string Comment { get; set; }
        public int? ReviwerMark { get; set; }
        public TestSummaryViewModel TestSummary { get; set; }
        public bool IsCorrect { get; set; }
    }
}