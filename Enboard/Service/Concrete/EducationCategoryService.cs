using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;


namespace Service
{
   public class EducationCategoryService:IEducationCategoryService
    {
       private IEducationCategoryRepository _IEducationCategoryRepository;

       public EducationCategoryService(IEducationCategoryRepository IEducationCategoryRepository)
       {
           this._IEducationCategoryRepository = IEducationCategoryRepository;
       }

        public IEnumerable<Master_EducationCategory> GetAll(Master_EducationCategory obj, string[] param, string spName)
        {
            return _IEducationCategoryRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_EducationCategory obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_EducationCategory GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_EducationCategory obj, string[] param, string spName)
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
