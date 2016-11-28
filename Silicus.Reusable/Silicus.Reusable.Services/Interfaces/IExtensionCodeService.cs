using Silicus.FrameworxProject.Models;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Services.Interfaces
{
    public interface IExtensionCodeService
    {
        List<Frameworx> GetAllFrameworx();
        List<FrameworxCategory> GetAllCategories();
        List<CodeType> GetAllCodeTypes();
        
        List<ExtensionSolution> GetAllExtensionSolution();
        List<ExtensionSolution> GetAllApprovedExtensionSolution();
        List<ExtensionSolution> GetMyAllExtensionSolution(int id);
        List<ExtensionSolution> GetAllReviewExtensionSolution(int id);

        List<OtherCode> GetAllOtherCodeList();
        List<OtherCode> GetMyAllOtherCodeList(int id);
        List<OtherCode> GetAllReviewOtherCodeList(int id);
        List<OtherCode> GetAllApprovedOtherCodeList();

        ExtensionSolution GetExtensionMethodById(int ExtensionId);
        void AddExtensionSolution(ExtensionSolution extensionSolution);

        void AddOtherCode(OtherCode otherCode);
        OtherCode GetOtherCodeMethodById(int ExtensionId);
        void EditExtensionSolution(ExtensionSolution extensionSolution);
        void EditOtherCode(OtherCode otherCode);
        void ExtensionFrequentSearchedCountUpdate(ExtensionSolution extensionSolution);
        void OtherCodeFrequentSearchedCountUpdate(OtherCode otherCode);
        void SendEmail(string userName, string ToEmailAddresses, string emailSubject,string codeType,string link);

        //  User GetUtilityUserByEmailId(string userEmailAddress);
        Task<bool> asyncSendEmail(string userName, string ToEmailAddresses, string emailSubject, string codeType, string link);
    }
}