using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Models
{
    public class UserWinningHistory
    {
        public int AwardId { get; set; }
        public string AwardMonth { get; set; }
        public string AwardYear { get; set; }
        public string ManagerComment { get; set; }
        public decimal AverageScore { get; set; }
        public string AdminComment { get; set; }
        public int NominationId { get; set; }
    }
}
