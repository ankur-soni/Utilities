using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

using Data;
using Models;


namespace Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository)
        {
            this._userRepository = UserRepository;
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LoginDetail> GetAll(LoginDetail obj, string[] param, string spName)
        {
            return _userRepository.GetAll(null, null, "");
        }
        public bool Insert(LoginDetail obj, string[] param, string spName)
        {
            return _userRepository.Insert(obj, param, spName);
        }

        public LoginDetail GetById(object Id)
        {

            return _userRepository.GetById(Id);
        }

        public bool Update(LoginDetail obj, string[] param, string spName)
        {
            return _userRepository.Update(obj, null, null);
        }

        public bool UpdateById(object Id)
        {
            return _userRepository.UpdateById(Id);
        }

        public List<GetDocumentDetails_Result> GetDocumentDetailsList()
        {

            List<GetDocumentDetails_Result> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.GetDocumentDetails().ToList();

            }
            return data;
        }

        public List<DocumentStatus_Result> DocumentStatusList(string userId)
        {

            List<DocumentStatus_Result> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.DocumentStatus(userId).ToList();

            }
            return data;
        }

        public bool DeleteUser(int ID)
        {

            bool result;

            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {

                    LoginDetail loginDetail = ctx.LoginDetails.Where(m => m.UserID == ID).FirstOrDefault();
                    //Code change - Column datatype changed from int to bool 
                    loginDetail.IsDelete = true;
                    EmployeeMaster empMaster = ctx.EmployeeMasters.Where(m => m.UserId == ID).FirstOrDefault();
                    if (empMaster != null)
                    {
                        empMaster.Active = 0;
                    }
                    ctx.SaveChanges();

                    result = true;
                }
                catch (Exception ex)
                {

                    result = false;
                }

            }
            return result;
        }

        public bool AddOfferCandidateToEmployee(EmployeeMaster empdataobj)
        {
            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {

                    ctx.EmployeeMasters.Add(empdataobj);
                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {

                    result = false;
                }

            }
            return result;
        }

        public bool UpdateEmployeeLeavingDetails(EmployeeMaster empdataobj)
        {

            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {
                    var empdetails = ctx.EmployeeMasters.Where(m => m.Id == empdataobj.Id).FirstOrDefault();

                    if (empdetails != null)
                    {
                        empdetails.ReasonForleaving = empdataobj.ReasonForleaving;
                        empdetails.LeavingDate = empdataobj.LeavingDate;
                        empdetails.Active = 0;
                        empdetails.JoiningDate = empdataobj.JoiningDate;
                        ctx.SaveChanges();
                    }
                    var logindetails = ctx.LoginDetails.Where(m => m.UserID == empdataobj.UserId).FirstOrDefault();
                    if (logindetails != null)
                    {
                        logindetails.IsActive = 0;

                    }

                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }

            }
            return result;
        }

        public bool UpdateEmployeeDetails(EmployeeMaster empdataobj)
        {

            bool result = false;
            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {
                    var empdetails = ctx.EmployeeMasters.Where(m => m.Id == empdataobj.Id).FirstOrDefault();
                    var logindetails = ctx.LoginDetails.Where(m => m.UserID == empdataobj.UserId).FirstOrDefault();
                    if (empdetails != null)
                    {
                        empdetails.DOB = empdataobj.DOB;
                        empdetails.Email = empdataobj.Email;
                        empdetails.EmpNo = empdataobj.EmpNo;
                        empdetails.EmployeeName = empdataobj.EmployeeName;
                        empdetails.JoiningDate = empdataobj.JoiningDate;
                        if (empdataobj.Active != null)
                            empdetails.Active = empdataobj.Active;
                        if (empdataobj.LeavingDate != null)
                            empdetails.LeavingDate = empdataobj.LeavingDate;
                    }
                    if (logindetails != null)
                    {
                        logindetails.DOB = empdataobj.DOB;
                        logindetails.Email = empdataobj.Email;
                        logindetails.IsDelete = empdataobj.Active == 1 ? false : true;
                        //string[] EmployeeName

                    }
                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }

            }
            return result;
        }

        public bool AddToUserActivation(int userId)
        {
            bool result = false;
            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {
                    var logindetails = ctx.LoginDetails.Where(m => m.UserID == userId).FirstOrDefault();
                    if (logindetails != null)
                    {

                        logindetails.IsDelete = false;
                        result = true;
                    }
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;

        }



        public string EncryptData(string ActualPwd)
        {

            string strmsg = string.Empty;
            byte[] encode = new byte[ActualPwd.Length];
            encode = Encoding.UTF8.GetBytes(ActualPwd);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;

            ////in this string you got the encrypted password

        }

        //Code change 

        /// <summary>
        ///   //Code change  Adding new method to fetch department name
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public string GetDepartmentName(int departmentId)
        {
            var data = "";

            if (departmentId != 0)
            {
                using (IPDEntities ctx = new IPDEntities())
                {
                    var list_data = ctx.Master_Department.Where(m => m.DepartmentID == departmentId).FirstOrDefault();
                    data = list_data.DepartmentName;
                }
            }
            return data;
        }

        /// <summary>
        ///   //Code change  Adding new method to fetch designation name
        /// <param name="designationId"></param>
        /// <returns></returns>
        public string GetDesignationName(int designationId)
        {
            var data = "";
            if (designationId != 0)
            {
                using (IPDEntities ctx = new IPDEntities())
                {
                    var list_data = ctx.Master_Designation.Where(m => m.DesignationID == designationId).FirstOrDefault();
                    data = list_data.Designation;
                }
            }
            return data;
        }

        /// <summary>
        /// Code change - added method to get sub education category for loggedin user on candidate side
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public int GetSubDocCategoryID(int userId)
        //{
        //    var data = 0;
        //    if (userId != 0)
        //    {
        //        using (IPDEntities ctx = new IPDEntities())
        //        {
        //            var list_data = ctx.LoginDetails.Where(l => l.UserID == userId).FirstOrDefault();
        //            data = list_data.SubDocCatID;
        //        }
        //    }
        //    return data;
        //}

        /// <summary>
        /// Code change - method to get education sub category list on candidate side 
        /// </summary>
        /// <returns></returns>
        //public List<Master_SubDocumentsCategory> GetEducationSubCategoriesList()
        //{
        //    List<Data.Master_SubDocumentsCategory> data = null;

        //    using (IPDEntities ctx = new IPDEntities())
        //    {
        //        data = ctx.Master_SubDocumentsCategory.Where(m => m.DocCatID == 4).ToList();

        //    }
        //    return data;
        
        //}

        /// <summary>
        /// Code change - method to get welocme note for admin
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetWelcomeNote(int userId)
        {
            var data = "";
            if (userId != 0)
            {
                using (IPDEntities ctx = new IPDEntities())
                {
                    var list_data = ctx.WelcomeNoteDetails.Where(m => m.UserID == userId && m.IsDelete != true).OrderByDescending(m => m.UpdatedDate).FirstOrDefault();
                    if (list_data != null)
                        data = string.IsNullOrEmpty(list_data.WelcomeNote) ? "" : list_data.WelcomeNote;
                    else data = "";
                }
            }
            return data;
        
        }

        /// <summary>
        /// Code change - method to update or add welcome note by admin
        /// </summary>
        /// <param name="welcomeobj"></param>
        /// <returns></returns>
        public bool UpdateWelcomeMessage(WelcomeModel welcomeobj)
        {

            bool result = false;
            using (IPDEntities ctx = new IPDEntities())
            {

                try
                {               
                    var welcomeNoteDetails = ctx.WelcomeNoteDetails.Where(m => m.UserID == welcomeobj.UserID && m.IsDelete != true).FirstOrDefault();
                    if (welcomeNoteDetails != null)
                    {
                        //entry exist
                        welcomeNoteDetails.IsDelete = true;
                        welcomeNoteDetails.UpdatedDate = DateTime.UtcNow;
                        welcomeNoteDetails.UpdatedBy = welcomeobj.UpdatedBy;
                        ctx.SaveChanges();
                    }

                    var newWelcomeNoteDetails = new WelcomeNoteDetail();
                    newWelcomeNoteDetails.WelcomeNote = string.IsNullOrEmpty(welcomeobj.WelcomeNote) ? "" : welcomeobj.WelcomeNote; 
                    newWelcomeNoteDetails.IsDelete = false;
                    newWelcomeNoteDetails.UserID = welcomeobj.UserID;
                    newWelcomeNoteDetails.UpdatedBy = welcomeobj.UpdatedBy;
                    newWelcomeNoteDetails.UpdatedDate = DateTime.UtcNow;
                    newWelcomeNoteDetails.CreatedBy = welcomeobj.CreatedBy;
                    newWelcomeNoteDetails.Createddate = DateTime.UtcNow;

                    ctx.WelcomeNoteDetails.Add(newWelcomeNoteDetails);
                    ctx.SaveChanges();

                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }

            }
            return result;
        }

        /// <summary>
        /// Code change - method to get latest welcome note from database to dispaly it for candidate 
        /// </summary>
        /// <returns></returns>
        public string GetLatestWelcomeNote()
        {
            var data = "";
            
                using (IPDEntities ctx = new IPDEntities())
                {
                    var list_data = ctx.WelcomeNoteDetails.Where(m => m.IsDelete != true).OrderByDescending(m => m.UpdatedDate).FirstOrDefault();
                    if (list_data != null)
                        data = string.IsNullOrEmpty(list_data.WelcomeNote) ? "" : list_data.WelcomeNote;
                    else data = "";
                }
           
            return data;

        }


        public List<Master_Bloodgroup> GetBloodGroupList()
        {
            List<Data.Master_Bloodgroup> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.Master_Bloodgroup.ToList();

            }
            return data;
        
        }

        public bool AddChangeRequestDetails(CandidateChangeRequestsDetail empdataobj)
        {
            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {
                    ctx.CandidateChangeRequestsDetails.Add(empdataobj);
                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }

        public List<string> GetPendingRequests(long userId)
        {
            List<string> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {          
                //neither reject nor approve
                data = ctx.CandidateChangeRequestsDetails.Where(c => c.IsApproved == null && c.UserID == userId).Select(c => c.FieldName).ToList();
            }
            return data;

        }

        public List<AdminEducationCategoryForUser> GetSelectedCategories(long userId)
        {
            List<Data.AdminEducationCategoryForUser> data = null;

            using (IPDEntities ctx = new IPDEntities())
            {
                data = ctx.AdminEducationCategoryForUsers.Where(a => a.UserID == userId && a.IsActive == true).ToList();

            }
            return data;

        }


        public bool AddEducationCategoryDetails(AdminEducationCategoryForUser empdataobj,string userName)
        {
            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {
                    ctx.AdminEducationCategoryForUsers.Add(empdataobj);
                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }

        public void UpdateEducationCategoryDetails(long userId, string userName)
        {
           
            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {
                    var existingDocCate = ctx.AdminEducationCategoryForUsers.Where(a => a.UserID == userId && a.IsActive == true).ToList();

                    if (existingDocCate != null)
                    {
                        foreach (var item in existingDocCate)
                        {
                            //entry exist
                            item.IsActive = false;
                            item.UpdatedBy = userName;
                            item.UpdatedDate = DateTime.UtcNow;
                            ctx.SaveChanges();
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
              
            }
           
        }

        public bool AddUserDetails(LoginDetail empdataobj)
        {
            bool result;
            using (IPDEntities ctx = new IPDEntities())
            {
                try
                {
                    ctx.LoginDetails.Add(empdataobj);
                    ctx.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                  result = false;
                  
                }
            }
            return result;
        }

        public ActivityDetails GetActivityDetails(long userId)
        {

            ActivityDetails data = new ActivityDetails();

            using (IPDEntities ctx = new IPDEntities())
            {
                data.PersonalDetailsDate = ctx.EmployeePersonalDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();
                data.ContactDetailsDate = ctx.EmployeeContactDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();
                data.EducationDetailsDate = ctx.EmployeeEducationDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();
                data.EmploymentDetailsDate = ctx.EmploymentDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();
                data.FamilyDetailsDate = ctx.EmployeeFamilyDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();
                data.UploadDocumentDetailsDate = ctx.DocumentDetails.Where(a => a.UserID == userId).OrderByDescending(m => m.UpdatedDate).Select(a => a.UpdatedDate).FirstOrDefault();

            } 
            return data;
        }


        public List<CandidateChangeRequestsDetail> GetCandidateChangeRequest(string searchString)
        {
            List<CandidateChangeRequestsDetail> result = new List<CandidateChangeRequestsDetail>();
            using (IPDEntities ctx = new IPDEntities())
            {
                if (string.IsNullOrWhiteSpace(searchString))
                {
                  result = ctx.CandidateChangeRequestsDetails.Include("LoginDetail").Where(x=>x.IsApproved == null).ToList();
                }
                else
                {
                    result = ctx.CandidateChangeRequestsDetails.Include("LoginDetail").Where(x => 
                        (x.IsApproved == null) 
                        && (x.LoginDetail.FirstName.ToLower() + " " + x.LoginDetail.LastName.ToLower()).Contains(searchString)).ToList();
                }
                
            }

            return result == null ? new List<CandidateChangeRequestsDetail>() : result;
        }

        public CandidateChangeRequestsDetail ChangeRequestAction(long CandChangeReqID, bool Action, string ActionTakenBy)
        {
            using (IPDEntities ctx = new IPDEntities())
            {
                var request = ctx.CandidateChangeRequestsDetails.Include("LoginDetail").Where(x => x.CandChangeReqID == CandChangeReqID).FirstOrDefault();
                try
                {
                    if (request != null)
                    {
                        request.IsApproved = Action;
                        if (Action)
                        {
                            switch (request.FieldName)
                            {
                                case "FirstName":
                                    request.LoginDetail.FirstName = request.FieldValue;
                                    break;
                                case "LastName":
                                    request.LoginDetail.LastName = request.FieldValue;
                                    break;
                                case "ContactNumber":
                                    request.LoginDetail.ContactNumber = request.FieldValue;
                                    break;
                                case "CountryCode":
                                    request.LoginDetail.CountryCode = request.FieldValue;
                                    break;
                                case "Email":
                                    request.LoginDetail.Email = request.FieldValue;
                                    break;
                                //case "NoOfEmployments":
                                //    request.LoginDetail.NoOfEmployments = Convert.ToInt32(request.FieldValue);
                                //    break;
                                case "DOB":
                                      string strmsg = string.Empty;
                                      byte[] encode = new byte[request.FieldValue.Length];
                                      encode = Encoding.UTF8.GetBytes(request.FieldValue);
                                      strmsg = Convert.ToBase64String(encode);
                                      request.LoginDetail.DOB = strmsg;
                                    break;
                                default:
                                    break;
                            }
                        }
                        request.UpdatedBy = ActionTakenBy;
                        request.UpdatedDate = DateTime.UtcNow;
                        ctx.SaveChanges();

                        return request;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    
                    return null;
                }

            }
        }

        public LoginDetail GetUserExists(string Firstsname , string Lastname , string Email)
        {

            using (IPDEntities ctx = new IPDEntities())
            {
                var result = ctx.LoginDetails.Where(T => T.FirstName == Firstsname && T.LastName == Lastname && T.Email == Email).FirstOrDefault();
                return result;
            }
            
        }
        public Master_Department GetDepartmentId(string Name)
        {

            using (IPDEntities ctx = new IPDEntities())
            {
                var result = ctx.Master_Department.Where(T => T.DepartmentName== Name).FirstOrDefault();
                return result;
            }

        }

        public List<LoginDetail> GetActiveUsers()
        {
            var users = new List<LoginDetail>();
            using (var context = new IPDEntities())
            {
                users = context.LoginDetails.Where(x => x.IsActive == 1 && x.RoleID == 16).ToList();
                return users;
            }
        }
    }
}
