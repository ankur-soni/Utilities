using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Web.ViewModel
{
    public class CubicleLocationCreateViewModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int CubicleLocationId { get; set; }

        //[Key]
        //[Column(Order = 1)]
        [Index("IX_Unique_CubicleLocation", 1, IsUnique = true)]
        [Required(ErrorMessage = "Building can't be blank")]
        [Display(Name = "Building *")]
        [StringLength(10)]
        public string Building { get; set; }

        //[Key]
        //[Column(Order = 2)]
        [Index("IX_Unique_CubicleLocation", 2, IsUnique = true)]
        [Required(ErrorMessage = "Floor Number can't be blank")]
        [Display(Name = "Floor Number *")]
        public int FloorNumber { get; set; }

        //[Key]
        //[Column(Order = 3)]
        [Index("IX_Unique_CubicleLocation", 3, IsUnique = true)]
        [Required(ErrorMessage = "Desk Number can't be blank")]
        [StringLength(15, ErrorMessage = "Desk Number should contain less than 15 characters")]
        [Display(Name = "Desk Number *")]
        public string DeskNumber { get; set; }
    }
}