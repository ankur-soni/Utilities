using System;
using System.Collections.Generic;
using Data;
using Repository;

namespace Service
{
    public class DocumentDetailsService :IDocumentDetailsService
    {
        private IDocumentDetailsRepository _IDocumentDetailsRepository;

        public DocumentDetailsService(IDocumentDetailsRepository IDocumentDetailsRepository)
        {
            this._IDocumentDetailsRepository = IDocumentDetailsRepository;
        }

        public IEnumerable<DocumentDetail> GetAll(DocumentDetail obj, string[] param, string spName)
        {
            return _IDocumentDetailsRepository.GetAll(obj, param, spName);
        }

        public bool Insert(DocumentDetail obj, string[] param, string spName)
        {
            return _IDocumentDetailsRepository.Insert(obj, param, spName);
        }

        public DocumentDetail GetById(object Id)
        {
            return _IDocumentDetailsRepository.GetById(Id);
        }

        public bool Update(DocumentDetail obj, string[] param, string spName)
        {
            return _IDocumentDetailsRepository.Update(obj, param, spName);
        }

        public bool UpdateById(object Id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _IDocumentDetailsRepository.Save();
        }

        public bool InsertDocDetails(out long ID, DocumentDetail obj, string[] param, string spName)
        {

            bool status = _IDocumentDetailsRepository.Insert(obj, param, spName);
            Save();
            ID = obj.DocDetID;
            return status;
        }
    }
}
