using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal static class SqlTypes
    {
        public static readonly string Xml = "xml";

        public static readonly string DateTime2 = "datetime2";

        public static readonly string Nvarchar = "nvarchar";

        public static readonly string Varchar = "varchar";

        public static readonly string BlobData = "varbinary(max)";
    }
}
