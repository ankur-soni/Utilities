using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;


namespace Service
{
   public interface IDocumentDetailsService:IService<DocumentDetail>
    {
       bool InsertDocDetails(out long ID, DocumentDetail obj, string[] param, string spName);
    }
}
