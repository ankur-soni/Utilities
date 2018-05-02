using Data;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;

namespace Service
{
    public class DocumentDetailsService : IDocumentDetailsService
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

        public bool Update(DocumentDetail obj, Expression<Func<DocumentDetail, object>> property)
        {
            return _IDocumentDetailsRepository.Update(obj, property);
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

        public List<DocumentDetail> GetDocumentDetailsByUserId(int userId = 0)
        {
            var documentDetails = new List<DocumentDetail>();
            if (userId > 0)
            {
                using (var context = new IPDEntities())
                {
                    documentDetails = context.DocumentDetails.Where(x => x.UserID == userId && x.IsActive==true).ToList();
                }
            }
            return documentDetails;
        }

        public List<DocumentDetail> GetDocumentDetails()
        {
            var documentDetails = new List<DocumentDetail>();
            try
            {
                using (var context = new IPDEntities())
                {
                    documentDetails = context.DocumentDetails.Select(x => new { DocumentName = x.DocumentName,
                        DocCatID = x.DocCatID,
                        DocDetID = x.DocDetID,
                        UserID = x.UserID,
                        IsActive = x.IsActive,
                        DocumentID = x.DocumentID,
                        IsAddressProof = x.IsAddressProof,
                        EmploymentDetID = x.EmploymentDetID
                    }).ToList().Select(d => new DocumentDetail {
                        DocumentName = d.DocumentName,
                        DocCatID = d.DocCatID,
                        DocDetID = d.DocDetID,
                        UserID = d.UserID,
                        IsActive = d.IsActive,
                        DocumentID = d.DocumentID,
                        IsAddressProof = d.IsAddressProof,
                        EmploymentDetID = d.EmploymentDetID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
               
            return documentDetails;
        }

        public bool SetInactive(DocumentDetail documentDetail)
        {
            var status = false;
            try
            {
                using (var context = new IPDEntities())
                {
                    var data = context.DocumentDetails.FirstOrDefault(x => x.DocumentID == documentDetail.DocumentID && x.DocDetID == documentDetail.DocDetID && x.UserID == documentDetail.UserID);
                    if (data != null)
                    {
                        data.IsActive = false;
                        data.UpdatedBy = HttpContext.Current.User.Identity.Name;
                        data.UpdatedDate = DateTime.UtcNow;
                        status = Convert.ToBoolean(context.SaveChanges());
                    }

                }
            }
            catch (Exception e)
            {

                throw;
            }
            
            return status;
        }

        public bool SetActive(DocumentDetail documentDetail)
        {
            var status = false;
            try
            {
                using (var context = new IPDEntities())
                {
                    var data = context.DocumentDetails.FirstOrDefault(x => x.DocumentID == documentDetail.DocumentID && x.DocDetID == documentDetail.DocDetID && x.UserID == documentDetail.UserID);
                    if (data != null)
                    {
                        data.IsActive = true;
                        data.UpdatedBy = HttpContext.Current.User.Identity.Name;
                        data.UpdatedDate = DateTime.UtcNow;
                        status = Convert.ToBoolean(context.SaveChanges());
                    }

                }
            }
            catch (Exception e)
            {

                throw;
            }

            return status;
        }
    }
}
