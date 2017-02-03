using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Reusable.Services
{
    public static class Helper
    {
        public static List<WorkItemClassificationNode> GetAllNodes(this WorkItemClassificationNode _self)
        {
            List<WorkItemClassificationNode> result = new List<WorkItemClassificationNode>();
            result.Add(_self);
            if (_self.Children != null)
                foreach (WorkItemClassificationNode child in _self.Children)
                {
                    result.AddRange(child.GetAllNodes());
                }
            return result;
        }
    }
}
