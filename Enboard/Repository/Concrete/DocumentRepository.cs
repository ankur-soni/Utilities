using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;
using Repository.Interface;


namespace Repository.Concrete
{
    public class DocumentRepository : RepositoryBase<Master_Document>, IDocumentRepository
    {
    }
}
