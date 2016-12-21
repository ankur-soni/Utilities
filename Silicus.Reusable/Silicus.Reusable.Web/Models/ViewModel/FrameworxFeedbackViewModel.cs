using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Reusable.Web.Models.ViewModel
{
    public class FrameworxFeedbackViewModel
    {
        public int Id { get; set; }

        public int FrameworxId { get; set; }

        public int UserId { get; set; }

        [MaxLength(150, ErrorMessage = Constants.MaxLengthMessage)]
        [StringLength(150, MinimumLength = 0, ErrorMessage = Constants.MaxLengthMessage)]
        [Required(ErrorMessage = Constants.RequiredFieldMessage)]
        public string Summary { get; set; }

        [Required(ErrorMessage = Constants.RequiredFieldMessage)]
        [AllowHtml]
        public string Description { get; set; }

        public string Type { get; set; }

        public string FeedBackFor { get; set; }

        public string OwnerName { get; set; }

        [EmailAddress]
        public string OwnerEmail { get; set; }
    }
}