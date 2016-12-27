﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Panel
    {

        [Key]
        public int PanelId { get; set; } 

        [StringLength(50)]
        [Required(ErrorMessage = "Panel is required!")]
        [Remote("IsDuplicatePositionName", "Positions", ErrorMessage = "Panel is already name exist !")]
        [Display(Name = "Position Name")]
        public string PanelName { get; set; } 
        public bool IsDeleted { get; set; }

        public Panel()
        {
            IsDeleted = false;
        }
    }
}
