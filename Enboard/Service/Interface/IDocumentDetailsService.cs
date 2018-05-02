using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Service
{
    public interface IDocumentDetailsService : IService<DocumentDetail>
    {
        bool InsertDocDetails(out long ID, DocumentDetail obj, string[] param, string spName);
        bool Update(DocumentDetail obj, Expression<Func<DocumentDetail, object>> property);
        List<DocumentDetail> GetDocumentDetailsByUserId(int userId);
        List<DocumentDetail> GetDocumentDetails();
        bool SetInactive(DocumentDetail documentDetail);
        bool SetActive(DocumentDetail documentDetail);
    }
}
