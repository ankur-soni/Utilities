using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Models.DataObjects
{
    public enum EngagementType
    {
       [Display(Name="T and M")]
       [Description("T and M")]
       T_and_M,

       [Display(Name="Fixed Price")]
       [Description("Fixed Price")]
       Fixed_Price,

       [Display(Name="Time Based")]
       [Description("Time Based")]
       Time_Based
    }
    
}