using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class PaymentDetails : BaseEntity
    {
        public PaymentDetails()
        {
            IsActive = true;
        }

        [Key]
        public int PaymentDetailId { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public decimal InvoicedEffort { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Required]
        public int WeekId { get; set; }

        public bool IsActive { get; set; }

        [UIHint("InvoiceStatusEditor")]
        public InvoiceStatus InvoiceStatus { get; set; }

        public int InvoiceStatusId { get; set; }
    }
}
