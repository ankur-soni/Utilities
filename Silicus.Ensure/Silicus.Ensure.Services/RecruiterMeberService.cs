using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services
{
    public class RecruiterMeberService : IRecruiterMeberService
    {
         private readonly IDataContext _context;

         public RecruiterMeberService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        // public RecruiterMembersDetail GetRecruiterMeberDetails(int userId)
        //{
        //    return _context.Query<RecruiterMembersDetail>().FirstOrDefault(x => x.UserId == userId);
        //}

        // public bool UpesertRecruiterMeberDetail(RecruiterMembersDetail recruiterMembersDetail)
        //{
        //    var recruiterMenber = _context.Query<RecruiterMembersDetail>().FirstOrDefault(x => x.UserId == recruiterMembersDetail.UserId);
        //    if (recruiterMenber==null)
        //    {
        //        _context.Add(recruiterMembersDetail);
        //    }
        //    else
        //    {
        //        recruiterMembersDetail.Id = recruiterMenber.Id;
        //        _context.Update(recruiterMembersDetail);
        //    }

        //    return true;
        //}
    }
}
