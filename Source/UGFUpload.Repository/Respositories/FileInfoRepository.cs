using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGFUpload.Repository.Contracts;
using UGFUpload.Repository.Data;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Repository.Respositories
{
    public class FileInfoRepository : IFileInfoRepository
    {
        private readonly BaseRepository<FileInfo> _rep;

        public FileInfoRepository(ApplicationDbContext db)
        {
            _rep = new BaseRepository<FileInfo>(db);
        }

        public bool Create(FileInfo file)
        {
            return _rep.Create(file);
        }

        public List<FileInfo> GetAllFiles()
        {
            return _rep.FindAll().ToList();
        }
    }
}
