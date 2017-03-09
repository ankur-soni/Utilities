﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class CandidateInfoViewModel
    {
        public string Name { get; set; }
        [Display(Name = "Requisition Id")]
        public string RequisitionId { get; set; }
        [Display(Name = "Total Experience")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal TotalExperience { get; set; }
        public string Position { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DOB { get; set; }
        public int TotalExperienceInYear { get; set; }
        public int TotalExperienceInMonth { get; set; }
    }
}