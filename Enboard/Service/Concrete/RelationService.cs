using Data;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class RelationService : IRelationService
    {
         private IRelationRepository _IRelationRepository;

         public RelationService(IRelationRepository IRelationRepository)
       {
           this._IRelationRepository = IRelationRepository;
       }

         public IEnumerable<Master_Relation> GetAll(Master_Relation obj, string[] param, string spName)
        {
            return _IRelationRepository.GetAll(obj, param, spName);
        }

         public bool Insert(Master_Relation obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

         public Master_Relation GetById(object Id)
        {
            throw new NotImplementedException();
        }

         public bool Update(Master_Relation obj, string[] param, string spName)
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
