using Data;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class CurrencyService : ICurrencyService
    {
          private ICurrencyService _ICurrencyService;

          public CurrencyService(ICurrencyService ICurrencyRepository)
        {
            this._ICurrencyService = ICurrencyRepository;
        }

          public IEnumerable<Master_Currency> GetAll(Master_Currency obj, string[] param, string spName)
        {
            return _ICurrencyService.GetAll(obj, param, spName);
        }

          public bool Insert(Master_Currency obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

          public Master_Currency GetById(object Id)
        {
            throw new NotImplementedException();
        }

          public bool Update(Master_Currency obj, string[] param, string spName)
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
