using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace HR_Web.ViewModel
{
    public class DocumentViewModel
    {
        public int PannelId { get; set; }

        public DocumentDetailModel DocumentDetailModel { set; get; }

        public SubDocumentCategoryModel SubDocumentCategoryModel { set; get; }

        public IEnumerable<DocumentModel> DocumentModelList { set; get; }

        public IEnumerable<DocumentDetailModel> DocumentDetailModelList { set; get; }

        public IEnumerable<SubDocumentCategoryModel> SubDocumentCategoryModelList { set; get; }

        public IEnumerable<EmployementModel> EmployementModelList { set; get; }

        public IEnumerable<DocumentModel> EducationDocCategoryList { set; get; }
    }
}