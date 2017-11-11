using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;
using Repository.Interface;

namespace Service.Concrete
{
    public class EducationDocumentCategoryMappingService : IEducationDocumentCategoryMappingService
    {
        private IEducationDocumentCategoryMappingRepository _IEducationDocumentCategoryMappingRepository;

        public EducationDocumentCategoryMappingService(IEducationDocumentCategoryMappingRepository iEducationDocumentCategoryMappingRepository)
        {
            _IEducationDocumentCategoryMappingRepository = iEducationDocumentCategoryMappingRepository;
        }
        public IEnumerable<EducationDocumentCategoryMapping> GetAll(EducationDocumentCategoryMapping obj, string[] param, string spName)
        {
            return _IEducationDocumentCategoryMappingRepository.GetAll(obj, param, spName);
        }

        public bool Insert(EducationDocumentCategoryMapping obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public EducationDocumentCategoryMapping GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(EducationDocumentCategoryMapping obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
