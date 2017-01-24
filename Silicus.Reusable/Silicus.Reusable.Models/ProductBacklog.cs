using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.FrameworxProject.Models
{
    public class ProductBacklog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string AreaPath { get; set; }
        public string AssigneeDisplayName { get; set; }
        public string AssigneeEmail { get; set; }
        public double TimeAllocated { get; set; }
        public double TimeSpent { get; set; }
        public string AssignedBy { get; set; }
    }
}
