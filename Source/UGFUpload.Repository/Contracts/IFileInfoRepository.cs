using System;
using System.Collections.Generic;
using System.Text;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Repository.Contracts
{
    public interface IFileInfoRepository
    {
        bool Create(FileInfo file);

        List<FileInfo> GetAllFiles();
    }
}
