//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    
    public partial class DocumentStatus_Result
    {
        public long DocDetID { get; set; }
        public long UserId { get; set; }
        public int DocCatID { get; set; }
        public int DocumentID { get; set; }
        public string Document { get; set; }
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public string CandidateName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerify { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}
