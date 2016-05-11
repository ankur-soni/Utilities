using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Models.DataObjects
{
    public class Nomination
    {
        public int Id { get; set; }
        public int AwardId { get; set; }
        public int ManagerId { get; set; }
        public int UserId { get; set; }
        public DateTime? NominationDate { get; set; }      //Date on which user is nominated
        public DateTime? CreatedDate { get; set; }        //Date on which winner selected

        [ForeignKey("AwardId")]
        public Award Award { get; set; }
    }
}
