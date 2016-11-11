using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID
        {
            get;
            set;
        }
        [Required]
        public string ProductName
        {
            get;
            set;
        }
        [Required]
        public string UnitPrice
        {
            get;
            set;
        }
    } 
}
