using Silicus.FrameworxProject.Models;
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
        List<Category> GetAllCategories();
        List<CodeType> GetAllCodeTypes();
        List<ExtensionSolution> GetAllExtensionSolution();
        List<OtherCode> GetAllOtherCodeList();
        ExtensionSolution GetExtensionMethodById(int ExtensionId);
        void AddExtensionSolution(ExtensionSolution extensionSolution);
        void AddOtherCode(OtherCode otherCode);
        OtherCode GetOtherCodeMethodById(int ExtensionId);
        void EditExtensionSolution(ExtensionSolution extensionSolution);
        void EditOtherCode(OtherCode otherCode);
        void ExtensionFrequentSearchedCountUpdate(ExtensionSolution extensionSolution);
        void OtherCodeFrequentSearchedCountUpdate(OtherCode otherCode);

    }
}