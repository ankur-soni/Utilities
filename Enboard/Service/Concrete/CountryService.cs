using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
    public class CountryService : ICountryService
    {
        private ICountryRepository _ICountryRepository;

        public CountryService(ICountryRepository ICountryRepository)
        {
            this._ICountryRepository = ICountryRepository;
        }

        public IEnumerable<Master_Country> GetAll(Master_Country obj, string[] param, string spName)
        {
            return _ICountryRepository.GetAll(obj, param, spName);
        }

        public bool Insert(Master_Country obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Country GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Country obj, string[] param, string spName)
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
