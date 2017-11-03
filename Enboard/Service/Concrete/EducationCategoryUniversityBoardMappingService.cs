using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository.Interface;
using Service.Interface;

namespace Service.Concrete
{
    public class EducationCategoryUniversityBoardMappingService : IEducationCategoryUniversityBoardMappingService
    {
        private IEducationCategoryUniversityBoardMappingRepository _IEducationCategoryUniversityBoardMappingRepository;

        public EducationCategoryUniversityBoardMappingService(IEducationCategoryUniversityBoardMappingRepository IEducationCategoryUniversityBoardMappingRepository)
        {
            this._IEducationCategoryUniversityBoardMappingRepository = IEducationCategoryUniversityBoardMappingRepository;
        }

        public IEnumerable<EducationCategoryUniversityBoardMapping> GetAll(EducationCategoryUniversityBoardMapping obj, string[] param, string spName)
        {
            return _IEducationCategoryUniversityBoardMappingRepository.GetAll(obj, param, spName);
        }

        public EducationCategoryUniversityBoardMapping GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EducationCategoryUniversityBoardMapping> GetUniversityBasedOnCatergory(int EducationCategoryID)
        {
            throw new NotImplementedException();

            
        }

        public bool Insert(EducationCategoryUniversityBoardMapping obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(EducationCategoryUniversityBoardMapping obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
