//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UtilityDataSyncLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwExt_TKT_Queue
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryMasterID { get; set; }
        public int QueueTypeMasterID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDefaultTicketQueue { get; set; }
        public bool IsActive { get; set; }
    }
}
