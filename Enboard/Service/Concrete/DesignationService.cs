using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

using Data;

namespace Service
{
    public class DesignationService : IDesignationService
    {
        private IDesignationRepository _designationRepository;

        public DesignationService(IDesignationRepository DesignationRepository)
        {
            this._designationRepository = DesignationRepository;
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Master_Designation> GetAll(Master_Designation obj, string[] param, string spName)
        {
            return _designationRepository.GetAll(null, null, "");
        }


        public bool Insert(Master_Designation obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

        public Master_Designation GetById(object Id)
        {

            throw new NotImplementedException();
        }

        public bool Update(Master_Designation obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }


        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }
        
        
    }
}
