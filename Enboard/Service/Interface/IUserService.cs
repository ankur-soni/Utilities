using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;

namespace Service
{
    public interface IUserService : IService<LoginDetail>
    {
        List<GetDocumentDetails_Result> GetDocumentDetailsList();

        List<DocumentStatus_Result> DocumentStatusList(string userId);

        bool DeleteUser(int ID);

        bool AddOfferCandidateToEmployee(EmployeeMaster empdataobj);
    

        bool UpdateEmployeeLeavingDetails(EmployeeMaster empdataobj);

        bool UpdateEmployeeDetails(EmployeeMaster empdataobj);
        bool AddToUserActivation(int userId);
        //Code change
        string GetDepartmentName(int departmentId);
        string GetDesignationName(int designationId);
        //int GetSubDocCategoryID(int userId);
        //List<Master_SubDocumentsCategory> GetEducationSubCategoriesList();
        string GetWelcomeNote(int userId);
        LoginDetail GetById(object userId);
        bool UpdateWelcomeMessage(WelcomeModel welcomeobj);
        string GetLatestWelcomeNote();

        List<Master_Bloodgroup> GetBloodGroupList();

        bool AddChangeRequestDetails(CandidateChangeRequestsDetail empdataobj);

        List<string> GetPendingRequests(long userId);

        List<AdminEducationCategoryForUser> GetSelectedCategories(long userId);

        bool AddEducationCategoryDetails(AdminEducationCategoryForUser empdataobj,string userName);

        bool AddUserDetails(LoginDetail empdataobj);

        void UpdateEducationCategoryDetails(long userId, string userName);

        ActivityDetails GetActivityDetails(long userId);

        List<CandidateChangeRequestsDetail> GetCandidateChangeRequest(string searchString);
        CandidateChangeRequestsDetail ChangeRequestAction(long CandChangeReqID, bool Action, string ActionTakenBy);
        
    }
}
