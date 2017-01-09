using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Models.DataObjects
{
    public enum EngagementType
    {
        [Display(Name = "None")]
        [Description("None")]
        None,

        [Display(Name = "Fixed Price")]
        [Description("Fixed Price")]
        Fixed_Price,

        [Display(Name = "T and M")]
        [Description("T and M")]
        T_and_M,

        [Display(Name = "Not To Exceed")]
        [Description("Not To Exceed")]
        Not_To_Exceed
    }

}