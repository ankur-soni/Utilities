using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Services
{
    public class ExtensionCodeService : IExtensionCodeService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;

        public ExtensionCodeService(Silicus.FrameworxProject.DAL.Interfaces.IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();
        }

        public List<Frameworx> GetAllFrameworx()
        {
            return _FrameworxProjectDatabaseContext.Query<Frameworx>().ToList();
        }

        public List<ExtensionSolution> GetAllExtensionSolution()
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().ToList();
        }

        public void AddExtensionSolution(ExtensionSolution extensionSolution)
        {
             _FrameworxProjectDatabaseContext.Add<ExtensionSolution>(extensionSolution); 
        }

        public void EditExtensionSolution(ExtensionSolution extensionSolution)
        {
            _FrameworxProjectDatabaseContext.Update<ExtensionSolution>(extensionSolution);
        }

        public void AddOtherCode(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Add<OtherCode>(otherCode);
        }

        public void EditOtherCode(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Update<OtherCode>(otherCode);
        }

        public List<Category> GetAllCategories()
        {
            return _FrameworxProjectDatabaseContext.Query<Category>().ToList();//Poulate Business Model h
        }

        public List<CodeType> GetAllCodeTypes()
        {
            return _FrameworxProjectDatabaseContext.Query<CodeType>().ToList();//Poulate Business Model h
        }

        public List<OtherCode> GetAllOtherCodeList()
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().ToList();
        }

        public ExtensionSolution GetExtensionMethodById(int ExtensionId)
        {
            return _FrameworxProjectDatabaseContext.Query<ExtensionSolution>().Where(exn => exn.Id == ExtensionId).First();

        }

        public OtherCode GetOtherCodeMethodById(int ExtensionId)
        {
            return _FrameworxProjectDatabaseContext.Query<OtherCode>().Where(exn => exn.Id == ExtensionId).First();

        }

        public void ExtensionFrequentSearchedCountUpdate(ExtensionSolution extensionSolution)
        {
            _FrameworxProjectDatabaseContext.Update<ExtensionSolution>(extensionSolution);
        }

        public void OtherCodeFrequentSearchedCountUpdate(OtherCode otherCode)
        {
            _FrameworxProjectDatabaseContext.Update<OtherCode>(otherCode);
        }
    }
}
