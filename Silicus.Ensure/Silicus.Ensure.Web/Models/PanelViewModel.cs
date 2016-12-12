using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class PanelViewModel
    {
        public int PanelId { get; set; }

        public string PanelName { get; set; }

        public bool IsAssignedPanel { get; set; }
    }
}