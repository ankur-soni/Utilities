using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Repository;

namespace Service
{
    public class DocumentCategoryService : IDocumentCategoryService
    {
        private IDocumentCategoryRepository _IDocumentCategoryRepository;

        public DocumentCategoryService(IDocumentCategoryRepository IDocumentCategoryRepository)
        {
            this._IDocumentCategoryRepository = IDocumentCategoryRepository;
        }

        public IEnumerable<Master_DocumentCategory> GetAll(Master_DocumentCategory obj, string[] param, string spName)
        {
            return _IDocumentCategoryRepository.GetAll(obj, param, spName);
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
