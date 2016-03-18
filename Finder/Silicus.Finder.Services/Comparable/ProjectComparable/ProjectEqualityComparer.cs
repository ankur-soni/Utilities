using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silicus.Finder.Models.DataObjects;
using System.Threading.Tasks;

namespace Silicus.Finder.Services.Comparable.ProjectComparable
{
    public class ProjectEqualityComparer : IEqualityComparer<Project>
    {
        public int GetHashCode(Project project)
        {
            return project.ProjectId.GetHashCode();
        }

        public bool Equals(Project project1, Project project2)
        {
            if (object.ReferenceEquals(project1, project2))
                return true;
            if (project1 == null || project2 == null)
                return false;
            return project1.ProjectId.Equals(project2.ProjectId);
        }

    }
}
