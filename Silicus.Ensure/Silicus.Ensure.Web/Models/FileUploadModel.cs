using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    class FileUploadModel
    {
        public HttpPostedFileBase File { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string FilePath { get; set; }
    }
}
