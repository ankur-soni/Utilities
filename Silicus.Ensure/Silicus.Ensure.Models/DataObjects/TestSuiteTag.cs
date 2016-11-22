using Silicus.Ensure.Models.CustomValidations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class TestSuiteTag
    {
        [Key]
        public int TestSuiteTagId { get; set; }        

        public int TagId { get; set; }

        public int Weightage { get; set; }

        public virtual TestSuite TestSuite { get; set; }
    }
}
