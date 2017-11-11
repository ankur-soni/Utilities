using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
  public  class CertificateService:ICertificateService
    {
      private ICertificateRepository _ICertificateRepository;

      public CertificateService(ICertificateRepository ICertificateRepository)
      {
          this._ICertificateRepository = ICertificateRepository;
      }

        public IEnumerable<Master_Certification> GetAll(Master_Certification obj, string[] param, string spName)
        {
            return _ICertificateRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Certification obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Certification GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Certification obj, string[] param, string spName)
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
