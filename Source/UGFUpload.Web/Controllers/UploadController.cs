using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using UGFUpload.Repository.Contracts;
using UGFUpload.Web.Models.Upload;

namespace UGFUpload.Web.Controllers
{
    public class UploadController : Controller
    {
        private readonly IFileInfoRepository _fileInfoRep;
        private readonly ICompareFileInfoRepository _compareFileRep;

        public UploadController(
            IFileInfoRepository fileInfoRep, 
            ICompareFileInfoRepository compareFileRep)
        {
            _fileInfoRep = fileInfoRep;
            _compareFileRep = compareFileRep;
        }

        public IActionResult Index()
        {
            var files = _fileInfoRep.GetAllFiles();
            var model = MapTo(files);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormFile fileInfo)
        {
            SaveFile(fileInfo);
            return RedirectToAction("Index");
        }

        public IActionResult CompareInfo()
        {
            var list = _compareFileRep.GetCompares();
            var model = MapTo(list);

            return View(model);
        }

        public IActionResult ViewPdf()
        {
            var list = _compareFileRep.GetCompares();
            var model = MapTo(list);

            return new ViewAsPdf(model);
        }

        #region Helpers

        private bool SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var fileInfo = new Repository.Entities.FileInfo()
            {
                FileName = file.Name,
                FullName = file.FileName,
                Length = file.Length,
                ContentType = file.ContentType
            };

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileInfo.Blob = ms.ToArray();
            }

            return _fileInfoRep.Create(fileInfo);
        }

        private List<FileInfoViewModel> MapTo(List<Repository.Entities.FileInfo> entityList)
        {
            var list = new List<FileInfoViewModel>();

            if (entityList == null || !entityList.Any())
                return list;

            list = entityList.Select(s => new FileInfoViewModel(s)).ToList();

            return list;
        }

        private List<CompareFileInfoViewModel> MapTo(List<Repository.Entities.CompareFileInfo> entityList)
        {
            var list = new List<CompareFileInfoViewModel>();

            if (entityList == null || !entityList.Any())
                return list;

            list = entityList.Select(s => new CompareFileInfoViewModel(s)).ToList();

            return list;
        }

        #endregion
    }
}
