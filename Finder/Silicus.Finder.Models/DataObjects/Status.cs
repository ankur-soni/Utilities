using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Models.DataObjects
{
    public enum Status
    {
        [Display(Name="Not Started")]
        [Description("Not Started")]
        Not_Started,

        [Display(Name = "On Going")]
        [Description("On Going")]
        On_Going,

        [Display(Name="Completed")]
        [Description("Completed")]
        Completed
    }
    
}
