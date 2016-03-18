using System;
using System.Linq;
using Silicus.ProjectTracker.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class PaymentDetailsViewModel : BaseEntity
    {
        public int PaymentDetailId { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        public decimal InvoicedEffort { get; set; }

        public int ProjectId { get; set; }
        
        public int WeekId { get; set; }

        [UIHint("InvoiceStatusEditor")]
        public InvoiceStatus InvoiceStatus { get; set; }

        public int InvoiceStatusId { get; set; }

        public string InvoiceStatusName { get; set; }
    }
}