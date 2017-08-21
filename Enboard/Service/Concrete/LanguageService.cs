using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public class LanguageService :ILanguageservice
    {
      private ILanguageRepository _ILanguageRepository;

      public LanguageService(ILanguageRepository ILanguageRepository)
      {
          this._ILanguageRepository = ILanguageRepository;
      }

      public IEnumerable<Master_Language> GetAll(Master_Language obj, string[] param, string spName)
        {
            return _ILanguageRepository.GetAll(obj, param, spName);
        }

      public bool Insert(Master_Language obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

      public Master_Language GetById(object Id)
        {
            throw new NotImplementedException();
        }

      public bool Update(Master_Language obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }


        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
