using Data;
using System;
using System.Linq.Expressions;

namespace Service
{
    public interface IDocumentDetailsService : IService<DocumentDetail>
    {
        bool InsertDocDetails(out long ID, DocumentDetail obj, string[] param, string spName);
        bool Update(DocumentDetail obj, Expression<Func<DocumentDetail, object>> property);
    }
}
