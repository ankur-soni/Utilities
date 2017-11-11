using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public  class SkillSetService:ISkillsetService
    {

      private ISkillSetRepository _ISkillSetRepository;

      public SkillSetService(ISkillSetRepository ISkillSetRepository)
      {
          _ISkillSetRepository = ISkillSetRepository;
      }

        public IEnumerable<Master_SkillSet> GetAll(Master_SkillSet obj, string[] param, string spName)
        {
            return _ISkillSetRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_SkillSet obj, string[] param, string spName)
        {
            return _ISkillSetRepository.Insert(obj,param,spName);
        }

        public Master_SkillSet GetById(object Id)
        {
            return _ISkillSetRepository.GetById(Id);
        }

        public bool Update(Master_SkillSet obj, string[] param, string spName)
        {
            return _ISkillSetRepository.Update(obj,param,spName); 
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _ISkillSetRepository.Save();
        }
    }
}
