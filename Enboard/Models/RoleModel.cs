using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
  public class RoleModel
    {
        public long Id { get; set; }
        public string PNo { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string ADUserName { get; set; }
        public bool IsActive { get; set; }
    }
}
