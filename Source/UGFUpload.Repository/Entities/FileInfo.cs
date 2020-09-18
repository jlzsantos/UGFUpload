using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UGFUpload.Repository.Entities
{
    public class FileInfo : EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        public long Length { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }

        [Required]
        public byte[] Blob { get; set; }
    }
}
