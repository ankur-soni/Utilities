using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Finder.Web.ViewModel
{
    public class CubicleLocationViewModel
    {
        public string Building { get; set; }
        
        [Display(Name = "Floor Number")]
        public int FloorNumber { get; set; }
        
        [Display(Name = "Desk Number")]
        public string DeskNumber { get; set; }
    }
}