using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Data;

namespace Models
{
    public class DocumentDetailListViewModel
    {
        public int PannelId { get; set; }
        public List<int> EducationCategoryList { get; set; }
        public List<EmploymentDetail> EmploymentDetailsList { get; set; }
        public List<Master_Document> Master_DocumentList { set; get; }
         public string HavePassport { get; set; }
        public bool IsAddressProof { get; set; }
        public bool IsIdProof { get; set; }
    }

    public class DocumentDetailModel
    {
        public long DocDetID { get; set; }
        public long UserID { get; set; }
        public int DocCatID { get; set; }
        public int SubDocCatID { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> IsUploaded { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerify { get; set; }
        public Nullable<bool> IsMailSent { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> EmploymentDetID { get; set; }
        public string MasterDocumentName { get; set; }

    }

    public class DocDetailsModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Nullable<int> DocCat_Pk { get; set; }
        public Nullable<int> SubDocCat_Pk { get; set; }
        public string SubCategory { get; set; }
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public string CandidateName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerify { get; set; }
        public string Maincategory { get; set; }
        public string Status { get; set; }

        public int MandatoryDocs { get; set; }
        public bool isAllApproved { get; set; }
        public bool isAllRejected { get; set; }
    }

    public class SaveUploadDocumentVewModel
    {
        public string DocCat_Id { get; set; }
        public string DocumentID { get; set; }
        public string DocumentCategory { get; set; }
        public string EmploymentDetID { get; set; }
        public string EmployementNo { get; set; }
        public bool IsAddressProof { get; set; }
        public bool IsIdProof { get; set; }
    }

}

