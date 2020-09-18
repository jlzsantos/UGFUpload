using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UGFUpload.Repository.Entities
{
    public class CompareFileInfo : EntityBase
    {
        public CompareFileInfo()
        {

        }

        public CompareFileInfo(CompareFileInfo entity)
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
