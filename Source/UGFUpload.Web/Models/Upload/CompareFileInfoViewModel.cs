using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Web.Models.Upload
{
    public class CompareFileInfoViewModel
    {
        public CompareFileInfoViewModel()
        {

        }

        public CompareFileInfoViewModel(CompareFileInfo entity)
        {
            Operator = entity.Operator;
            OperatorValue = entity.OperatorValue;
            ExecutionDate = entity.ExecutionDate;
        }

        public int Operator { get; set; }

        public decimal OperatorValue { get; set; }

        public DateTime ExecutionDate { get; set; }
    }
}
