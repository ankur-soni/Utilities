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
    public class DocumentService : IDocumentService
    {
         private IDocumentRepository _IDocumentRepository;

         public DocumentService(IDocumentRepository IDocumentRepository)
       {
           this._IDocumentRepository = IDocumentRepository;
       }

         public IEnumerable<Master_Document> GetAll(Master_Document obj, string[] param, string spName)
        {
            return _IDocumentRepository.GetAll(obj, param, spName);
        }

         public bool Insert(Master_Document obj, string[] param, string spName)
        {
            throw new NotImplementedException();
        }

         public Master_Document GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Master_Document obj, string[] param, string spName)
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
