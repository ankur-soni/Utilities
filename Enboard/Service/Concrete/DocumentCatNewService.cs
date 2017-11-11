using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
   public class DocumentCatNewService:IDocumentCatNewService
    {
       private IDocumentCatNewRepository _IDocumentCatNewRepository;

       public DocumentCatNewService(IDocumentCatNewRepository IDocumentCatNewRepository)
       {
           this._IDocumentCatNewRepository = IDocumentCatNewRepository;
       }

       public IEnumerable<Master_DocumentCategory> GetAll(Master_DocumentCategory obj, string[] param, string spName)
        {
            return _IDocumentCatNewRepository.GetAll(obj, param, spName);
        }

       public bool Insert(Master_DocumentCategory obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

       public Master_DocumentCategory GetById(object Id)
        {
            throw new NotImplementedException();
        }

       public bool Update(Master_DocumentCategory obj, string[] param, string spName)
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
