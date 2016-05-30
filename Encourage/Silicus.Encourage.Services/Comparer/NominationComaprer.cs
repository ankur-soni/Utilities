using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Comparer
{
    class NominationComparer : IEqualityComparer<Nomination>
    {

        public NominationComparer()
        {
                
        }
        public bool Equals(Nomination x, Nomination y)
        {
            return
              x.Id == x.Id;
        }

        public int GetHashCode(Nomination obj)
        {
            return obj.GetHashCode();
        }
    }
}
