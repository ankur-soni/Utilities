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
    using System.Collections.Generic;
    
    public partial class DocumentDetail
    {
        public long DocDetID { get; set; }
        public long UserID { get; set; }
        public int DocCatID { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string FilePath { get; set; }
        public bool IsAddressProof { get; set; }
        public bool IsIdProof { get; set; }
        public Nullable<long> EmploymentDetID { get; set; }
        public Nullable<bool> IsUploaded { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerify { get; set; }
        public Nullable<bool> IsMailSent { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Master_Document Master_Document { get; set; }
        public virtual Master_DocumentCategory Master_DocumentCategory { get; set; }
        public virtual Master_DocumentCategory Master_DocumentCategory1 { get; set; }
        public virtual LoginDetail LoginDetail { get; set; }
    }
}
