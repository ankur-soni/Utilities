using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Models;
using Service;
using Data;
using System.IO;
using HR_Web.ViewModel;
using Service.Interface;
using HR_Web.Utilities;

namespace HR_Web.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        int userId = 0;
        string userName = null;

        private IRelationService _IRelationService;
        private IEmployementService _IEmployementService;
        private IDocumentDetailsService _IDocumentDetailsService;
        private IDocumentCategoryService _IDocumentCategoryService;
        private ICandidateProgressDetailService _ICandidateProgressDetailService;
        private IEducationDocumentCategoryMappingService _IEducationDocumentCategoryMappingService;
        private IEmployeeService _IEmployeeService;
        private IPersonalService _IPersonalService; 
        private IEmploymentCountService _IEmploymentCountService;

        //Code change - EDMX Fix 
        private IDocumentService _IDocumentService;
        private IUserService _IUserService;

        Master_DocumentCategory _DocumetCat;
        DocumentDetail _docDetails;
        Master_DocumentCategory _DocumetCatNew;
        EducationDocumentCategoryMapping _educationDocumentCategoryMapping;
        EmploymentDetail _employment;
        Master_Document _document;
        LoginDetail _loginDetails;

        public DocumentController(
            IUserService IUserService,
            IRelationService IRelationService,
            IDocumentService IDocumentService,
            IEmployementService IEmployementService,
            IDocumentDetailsService IDocumentDetailsService,
            IDocumentCategoryService IDocumentCategoryService,
            IEducationDocumentCategoryMappingService IEducationDocumentCategoryMappingService, ICandidateProgressDetailService ICandidateProgressDetailService, IPersonalService IPersonalService, IEmployeeService IEmployeeService, IEmploymentCountService IEmploymentCountService)
        {
            _IUserService = IUserService;
            _IRelationService = IRelationService;
            _IEmployementService = IEmployementService;
            _IDocumentDetailsService = IDocumentDetailsService;
            _IDocumentCategoryService = IDocumentCategoryService;
            _IDocumentService = IDocumentService;
            _IEducationDocumentCategoryMappingService = IEducationDocumentCategoryMappingService;
            _ICandidateProgressDetailService = ICandidateProgressDetailService;
            _IPersonalService = IPersonalService;
            _IEmploymentCountService = IEmploymentCountService;

            _loginDetails = new LoginDetail();
            _document = new Master_Document();
            _docDetails = new DocumentDetail();
            _employment = new EmploymentDetail();
            _DocumetCat = new Master_DocumentCategory();
            _DocumetCatNew = new Master_DocumentCategory();
            _educationDocumentCategoryMapping = new EducationDocumentCategoryMapping();
            _IEmployeeService = IEmployeeService;
        }

        [HttpGet]
        [Authorize]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index(string DocCatID)
        {
            List<Master_Document> DocumentDetaillist = new List<Master_Document>();

            ViewBag.WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"].ToString();
            ViewBag.WebUrlUploadedFolder = System.Configuration.ConfigurationManager.AppSettings["WebUrlUploadedFolder"].ToString();
            ViewBag.DocumentPath = System.Configuration.ConfigurationManager.AppSettings["DocumentPath"].ToString();
            ViewBag.DocumentCategory = GetDocumentCategory();



            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                ViewBag.userId = userId;
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                DocumentDetaillist = _IDocumentService.GetAll(null, null, "").ToList();
            }
            ViewBag.DocCatID = DocCatID;
            var userDetails = _IUserService.GetById(userId);
            ViewBag.IsSubmitted = userDetails == null ? false : userDetails.IsSubmitted.HasValue && userDetails.IsSubmitted.Value;
            return View(new DocumentDetailListViewModel()
            {
                Master_DocumentList = DocumentDetaillist,
                EmploymentDetailsList = GetEmploymentDetailsList(userId),
                EducationCategoryList = GetEducationCategoryList(userId)
            });
        }

        private List<EmploymentDetail> GetEmploymentDetailsList(int userId)
        {
            return _IEmployementService.GetEmploymnetByUser(userId);
        }

        private List<int> GetEducationCategoryList(int userId)
        {
            List<int> educationCategoryList = new List<int>();
            var userEducationCategoryList = _IUserService.GetById(userId);
            if (userEducationCategoryList != null)
            {
                var educationDocumentCategoryMappingList = _IEducationDocumentCategoryMappingService.GetAll(null, null, "");
                if (userEducationCategoryList.AdminEducationCategoryForUsers.Any() && educationDocumentCategoryMappingList.Any())
                {
                    var adminEducationCategoryForUsers = userEducationCategoryList.AdminEducationCategoryForUsers.Where(x => x.IsActive.Value == true);
                    educationCategoryList = (from educationItem in adminEducationCategoryForUsers
                                             join mappingItem in educationDocumentCategoryMappingList
                                             on educationItem.EducationCategoryId equals mappingItem.EducationCategoryID
                                             select mappingItem.DocumentID).ToList();
                }
            }
            return educationCategoryList;
        }

        [HttpPost]
        public ActionResult SaveUploadedDocuments(SaveUploadDocumentVewModel saveUploadDocumentVewModel)
        {
            if (saveUploadDocumentVewModel.EmployementNo == null)
            {
                saveUploadDocumentVewModel.EmployementNo = null;
            }
            bool status = false;
            string path;
            long id = 0;
            string DodIDs = null;

            ViewBag.WebUrlUploadedFolder = System.Configuration.ConfigurationManager.AppSettings["WebUrlUploadedFolder"].ToString();
            ViewBag.WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"].ToString();
            ViewBag.DocumentPath = System.Configuration.ConfigurationManager.AppSettings["DocumentPath"].ToString();

            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            var docDetails_lst = _IDocumentDetailsService.GetAll(null, null, "");

            for (int i = 0; i < Request.Files.Count; i++)
            {

                int maxSize = 2048;

                if (string.IsNullOrEmpty(saveUploadDocumentVewModel.DocCat_Id) && string.IsNullOrEmpty(saveUploadDocumentVewModel.DocumentID) && string.IsNullOrEmpty(saveUploadDocumentVewModel.DocumentCategory))
                {
                    return Json(status);
                }
                else
                {
                    var file = Request.Files[i];

                    int sizeInBytes = file.ContentLength;

                    if ((sizeInBytes / 1024) > maxSize)
                    {
                        return Content("Please select file having size less than 2 MB.");
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    string extension = System.IO.Path.GetExtension(file.FileName);

                    if (extension.ToLower() == ".jpg".ToLower() || extension.ToLower() == ".doc".ToLower() || extension.ToLower() == ".docx".ToLower() || extension.ToLower() == ".jpeg".ToLower() || extension.ToLower() == ".pdf".ToLower())
                    {

                        path = Path.Combine(Server.MapPath("~/UploadedDocuments/"), fileName);
                        file.SaveAs(path);

                        if (string.IsNullOrEmpty(saveUploadDocumentVewModel.EmployementNo))
                        {
                            var obj = docDetails_lst.Where(u => u.UserID == userId && u.DocCatID == Convert.ToInt32(saveUploadDocumentVewModel.DocCat_Id) && u.IsActive == true).ToList();
                            if (obj != null && obj.Count > 0)
                            {
                                foreach (var item in obj)
                                {
                                    item.IsActive = true;
                                    status = _IDocumentDetailsService.Update(item, null, "");
                                }

                            }
                        }

                        _docDetails.UserID = userId;
                        _docDetails.DocCatID = Convert.ToInt32(saveUploadDocumentVewModel.DocCat_Id);
                        _docDetails.DocumentName = fileName;
                        _docDetails.ContentType = extension;
                        _docDetails.FilePath = ViewBag.WebUrlUploadedFolder + userName + "/" + fileName; ;
                        _docDetails.IsUploaded = true;
                        _docDetails.UpdatedBy = userName;
                        _docDetails.UpdatedDate = DateTime.Now;
                        _docDetails.CreatedBy = userName;
                        _docDetails.CreatedDate = DateTime.Now;
                        _docDetails.IsActive = true;
                        _docDetails.Data = FileToByteArray(fileName, path);

                        _docDetails.DocumentID = Convert.ToInt32(saveUploadDocumentVewModel.DocumentID);
                        if (saveUploadDocumentVewModel.EmploymentDetID != "")
                        {
                            _docDetails.EmploymentDetID = Convert.ToInt32(saveUploadDocumentVewModel.EmploymentDetID);
                        }
                        status = _IDocumentDetailsService.InsertDocDetails(out id, _docDetails, null, "");
                        DodIDs += id.ToString() + " ";
                    }
                    else
                    {
                        return Json(status);
                    }
                }

            }
            var result = new {DocCatID = saveUploadDocumentVewModel.DocCat_Id ,  status = status, CurrDocId = DodIDs, DocumentName = _docDetails.DocumentName };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //Code change - EDMX Fix 
        [HttpGet]
        public ActionResult CheckDuplicateFileName(string DocCat_Id, string DocumentID, string fileName, string EmploymentDetailID)
        {
            var docDetails_lst = _IDocumentDetailsService.GetAll(null, null, "");
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            long longEmploymentDetailID;
            long.TryParse(EmploymentDetailID,out longEmploymentDetailID);
            var obj = docDetails_lst.Where(u =>u.EmploymentDetID == longEmploymentDetailID && u.UserID == userId && u.DocumentID == Convert.ToInt32(DocumentID) && u.DocCatID == Convert.ToInt32(DocCat_Id) && u.IsActive == true && u.DocumentName == fileName).ToList();
            if (obj.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public byte[] FileToByteArray(string fileName, string path)
        {
            byte[] fileContent = null;

            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
            long byteLength = new System.IO.FileInfo(path).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();

            return fileContent;
        }


        public FileContentResult getImg(int id)
        {
            byte[] byteArray;
            DocumentDetail Doc = _IDocumentDetailsService.GetById(id);
            byteArray = Doc.Data;
            string strContentType = string.Empty;
            if (Doc.ContentType == ".pdf")
                strContentType = "application/pdf";
            else
                strContentType = "image/jpg";
            return byteArray != null ? new FileContentResult(byteArray, strContentType) : null;
        }

        public ActionResult getDocument(int id)
        {  
            byte[] byteArray;
            DocumentDetail Doc = _IDocumentDetailsService.GetById(id);
            byteArray = Doc.Data;
            string strContentType = string.Empty;
            if (Doc.ContentType == ".docx" || Doc.ContentType ==".doc")
            {
                if ( Doc.ContentType ==".doc")
                {
                    strContentType = "application/msword";
                }
                else
                {
                    strContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                }
               
            }
            else
            {
                if (Doc.ContentType == ".pdf")
                    strContentType = "application/pdf";
                else
                    strContentType = "image/jpg";
            }
            
            if (byteArray != null)
            {
                return this.File(byteArray, strContentType,Doc.DocumentName);    
            }
            else
            {
                return new EmptyResult();
            }
            
        }

        [HttpPost]
        public ActionResult DeleteUploadedDocuments(string DocumentID = "", string EmployementNo = "")
        {
            bool status = false;
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

            var docDetails_lst = _IDocumentDetailsService.GetAll(null, null, "");
            if (!string.IsNullOrEmpty(DocumentID) && string.IsNullOrEmpty(EmployementNo))
            {
                //Code change - EDMX Fix 
                var obj = docDetails_lst.Where(u => u.UserID == userId && u.DocDetID == Convert.ToInt32(DocumentID) && u.IsActive == true).ToList();
                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        item.IsActive = false;
                        item.UpdatedBy = userName;
                        item.UpdatedDate = DateTime.Now;
                        status = _IDocumentDetailsService.Update(item, null, "");
                    }
                }

                return Json(new { status = status, DocCatID =obj!= null ? obj.SingleOrDefault().DocCatID:1  });

            }
            else
            {
                //EDMX Fix - Commenting below code as its not in use 
                var obj = docDetails_lst.Where(u => u.UserID == userId && u.IsActive == true).ToList();
                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        item.IsActive = false;
                        status = _IDocumentDetailsService.Update(item, null, "");
                    }
                }

                return Json(new { status = status, DocCatID = obj != null ? obj.SingleOrDefault().DocCatID : 1 });
            }
         
        }

        [HttpGet]
        public ActionResult EmployeeAttachments()
        {
            return View();
        }

        [ChildActionOnly]

        public SelectList GetDocumentCategory()
        {
            List<Master_DocumentCategory> List = new List<Master_DocumentCategory>();

            List = _IDocumentCategoryService.GetAll(null, null, "").ToList();

            //Code change - EDMX Fix 
            SelectList selList = new SelectList(List, "DocCatID", "DocumentCategoryName");
            return selList;
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        public ActionResult GetCandidateProgressBar()
        {
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            CandidateProgressDetails candidateProgressDetails = new CandidateProgressDetails(_IUserService, _IRelationService, _ICandidateProgressDetailService, _IEmploymentCountService);

            CandidateGraphProgressDetailViewModel candidateGraphProgressDetailViewModel = candidateProgressDetails.SaveCandidateProgressDetails(userId);

            return View(candidateGraphProgressDetailViewModel);
        }

        public ActionResult FinalStatus()
        {
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            var userDetails = _IUserService.GetById(userId);
            var relationList = _IRelationService.GetAll(null, null, "");
            FinalSubmitViewModel finalSubmitViewModel = new FinalSubmitViewModel();
            if (userDetails != null)
            {
                finalSubmitViewModel.FinalStatus = userDetails.IsSubmitted == null ? false : userDetails.IsSubmitted.Value;
                var personalDetail = userDetails.EmployeePersonalDetails.FirstOrDefault(x => x.IsActive == true);
                var contactDetail = userDetails.EmployeeContactDetails.FirstOrDefault();
                var userEmployemntList = userDetails.EmploymentDetails.Where(x => x.IsActive == true);
                var assignedEducationCategaries = userDetails.AdminEducationCategoryForUsers.Where(x => x.IsActive == true).ToList();
                var userEducationList = userDetails.EmployeeEducationDetails.Where(x => x.IsActive == true);
                var documentDetails = userDetails.DocumentDetails.Where(x => x.IsActive == true);
                var familydetails = userDetails.EmployeeFamilyDetails.Where(x => x.IsActive == true).ToList();
                relationList = relationList.Where(r => r.RelationID != Constants.Spouse && r.RelationID != Constants.Child).ToList();

                if (personalDetail == null)
                {
                    finalSubmitViewModel.PersonalDetailsError = "Personal Details";
                }

                if (contactDetail == null)
                {
                    finalSubmitViewModel.ContactDetailsError = "Contact Details";

                }

                if (!userEmployemntList.Any(x => x.IsCurrentEmployment == true))
                {
                        finalSubmitViewModel.EmploymentDetailsError = "Employment Details";
                }

                if (userEducationList.Any() && assignedEducationCategaries.Any())
                {

                    assignedEducationCategaries.Remove(assignedEducationCategaries.FirstOrDefault(x => x.EducationCategoryId == 8));

                    if (userEducationList.Where(x => x.EducationCategoryID != 8).Count() != assignedEducationCategaries.Count)
                    {
                        finalSubmitViewModel.EducationDetailsError = "Education Details";
                    }
                }
                else
                {
                    finalSubmitViewModel.EducationDetailsError = "Education Details";
                }

                if (familydetails.Any())
                {
                    if (personalDetail != null && personalDetail.MaritalStatID == Constants.Single)
                    {
                        relationList = relationList.Where(r => r.RelationID != Constants.Spouse && r.RelationID != Constants.Child && r.RelationID != Constants.Sibling && r.RelationID != Constants.Relative).ToList();

                        if (familydetails.Where(a => a.RelationshipID == Constants.Father || a.RelationshipID ==  Constants.Mother).ToList().Count != relationList.Count())
                        {
                            finalSubmitViewModel.FamilyDetailsError = "Family Details";
                        }
                    }
                    else if (personalDetail != null && personalDetail.MaritalStatID == Constants.Married)
                    {

                        relationList = relationList.Where(r => r.RelationID != Constants.Child && r.RelationID != Constants.Sibling && r.RelationID != Constants.Relative).ToList();

                        if (familydetails.Count <= relationList.Count())
                        {
                            finalSubmitViewModel.FamilyDetailsError = "Family Details";
                        }
                    }

                }
                else
                {
                    finalSubmitViewModel.FamilyDetailsError = "Family Details";
                }
                if (documentDetails.Any() && documentDetails.Count() != 0)
                {
                    var docValidation = true;
                    
                    if (documentDetails.FirstOrDefault(x => x.DocumentID == Constant.IDProof.PANCard) == null)
                    {
                        docValidation = false;
                            
                    }

                  
                    foreach (var item in userDetails.AdminEducationCategoryForUsers)
                    {
                        var education= item.Master_EducationCategory.EducationDocumentCategoryMappings.FirstOrDefault();
                        if (education != null)
                        {
                            var docid = education.DocumentID;
                            if (!documentDetails.Any(x => x.DocumentID == docid))
                            {
                                docValidation = false;
                            }
                        }
                    }


                    foreach (var item in userEmployemntList)
                    {
                        if (item.IsCurrentEmployment.HasValue && item.IsCurrentEmployment.Value)
                        {
                            if (!documentDetails.Any(x => x.EmploymentDetID == item.EmploymentDetID && x.DocumentID == Constant.EmploymentProof.Latest3PaySlips))
                            {
                                docValidation = false;
                            }
                        }
                        else
                        {
                            if (!documentDetails.Any(x => x.EmploymentDetID == item.EmploymentDetID && (x.DocumentID == Constant.EmploymentProof.Latest3PaySlips)))
                            {
                                docValidation = false;
                            }

                            if (!documentDetails.Any(x => x.EmploymentDetID == item.EmploymentDetID && (x.DocumentID == Constant.EmploymentProof.ExperienceLetter)))
                            {
                                docValidation = false;
                            }


                        }
                    }

                    if (!docValidation)
                    {
                        finalSubmitViewModel.UploadDetailsError = "Upload Documents";
                    }
                }
                else
                {
                    finalSubmitViewModel.UploadDetailsError = "Upload Documents";
                }
                


            }
            return View(finalSubmitViewModel);
        }

        [HttpPost]
        public ActionResult FinalStatus(FinalSubmitViewModel finalSubmitViewModel)
        {
            if (Convert.ToBoolean(Convert.ToString(finalSubmitViewModel.FinalStatus)))
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                var userDetails = _IUserService.GetById(userId);
                userDetails.IsSubmitted = true;
                _IUserService.Update(userDetails, null, "");

                return RedirectToAction("Welcome", "Home");
            }

            return RedirectToAction("FinalStatus", "Document");
        }

        public ActionResult Learn()
        {
            return View();
        }

        public ActionResult Support()
        {
            var Employee = _IEmployeeService.GetAll(null, null, "").Where(m => m.UserId == SessionManager.UserId).FirstOrDefault();
            bool IsOnboarded = Employee == null ? false : true;
            ViewBag.IsOnboarded = IsOnboarded;
            return View();
        }
    }
}