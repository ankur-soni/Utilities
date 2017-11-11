using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository.Interface;

namespace Service.Concrete
{
    public class CandidateProgressDetailService : ICandidateProgressDetailService
    {
        private ICandidateProgressDetailRepository _ICandidateProgressDetailRepository;

        public CandidateProgressDetailService(ICandidateProgressDetailRepository iCandidateProgressDetailRepository)
        {
            _ICandidateProgressDetailRepository = iCandidateProgressDetailRepository;
        }
        public IEnumerable<CandidateGraphProgressDetail> GetAll(CandidateGraphProgressDetail obj, string[] param, string spName)
        {
            return _ICandidateProgressDetailRepository.GetAll(obj, param, spName);
        }

        public bool Insert(CandidateGraphProgressDetail obj, string[] param, string spName)
        {
            return _ICandidateProgressDetailRepository.Insert(obj, param, spName);
        }

        public CandidateGraphProgressDetail GetById(object Id)
        {
            return _ICandidateProgressDetailRepository.GetById(Id);
        }

        public bool Update(CandidateGraphProgressDetail obj, string[] param, string spName)
        {
            return _ICandidateProgressDetailRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            return _ICandidateProgressDetailRepository.UpdateById(Id);
        }

        public bool Save()
        {
            return _ICandidateProgressDetailRepository.Save();
        }
    }
}
