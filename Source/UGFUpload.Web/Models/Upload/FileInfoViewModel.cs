using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGFUpload.Web.Models.Upload
{
    public class FileInfoViewModel
    {
        public FileInfoViewModel()
        {

        }

        public FileInfoViewModel(Repository.Entities.FileInfo fileInfo)
        {
            FullName = fileInfo.FullName;
            CreatedDateTime = fileInfo.CreatedDate;
        }

        public string FullName { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
