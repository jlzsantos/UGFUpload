using System;
using System.Collections.Generic;
using System.Text;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Repository.Contracts
{
    public interface ICompareFileInfoRepository
    {
        List<CompareFileInfo> GetCompares();
    }
}
