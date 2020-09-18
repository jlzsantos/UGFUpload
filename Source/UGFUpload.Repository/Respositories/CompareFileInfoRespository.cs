using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UGFUpload.Repository.Contracts;
using UGFUpload.Repository.Data;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Repository.Respositories
{
    public class CompareFileInfoRespository : ICompareFileInfoRepository
    {
        private readonly BaseRepository<Entities.FileInfo> _fileInfoRep;

        public CompareFileInfoRespository(ApplicationDbContext db)
        {
            _fileInfoRep = new BaseRepository<Entities.FileInfo>(db);
        }

        public List<CompareFileInfo> GetCompares()
        {
            var list = ProcessCompares();
            return list
                .OrderBy(x => x.Operator)
                .ToList();
        }

        private List<CompareFileInfo> ProcessCompares()
        {
            var result = new List<CompareFileInfo>();
            var list = MapCompares();

            var groupedOperators = list.GroupBy(x => x.Operator);

            foreach (var groupItem in groupedOperators)
            {
                if (groupItem.Count() > 1)
                {
                    var maxItemValue = groupItem.OrderBy(x => x.OperatorValue).Last();
                    result.Add(new CompareFileInfo(maxItemValue));
                }
            }

            return result;
        }

        private List<CompareFileInfo> MapCompares()
        {
            var provider = CultureInfo.GetCultureInfo("pt-Br");
            int id = 1;
            var list = new List<CompareFileInfo>();
            var files = _fileInfoRep.FindAll()
                .OrderByDescending(x => x.Id)
                .Take(2)
                .ToList();

            if (files == null || !files.Any() || files.Count() < 2)
                throw new Exception("Não há arquivos para serem comparados.");

            foreach (var file in files)
            {
                using (MemoryStream ms = new MemoryStream(file.Blob))
                {
                    string csvFileContent = "";
                    using (StreamReader reader = new StreamReader(ms))
                    {
                        csvFileContent = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(csvFileContent))
                    {
                        var csvListValues = new List<string[]>();
                        string[] csvFileLines = csvFileContent.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        foreach (string fileItem in csvFileLines)
                        {
                            csvListValues.Add(fileItem.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));                            
                        }

                        if (csvListValues.Any())
                        {
                            for (int i = 0; i < csvListValues.Count(); i++)
                            {
                                // Para ignorar o cabeçalho
                                if (i > 0)
                                {
                                    var newItem = new CompareFileInfo()
                                    {
                                        Id = id,
                                        Operator = Convert.ToInt32(csvListValues[i][0]),
                                        OperatorValue = GetOperatorValue(csvListValues[i][1]),
                                        ExecutionDate = Convert.ToDateTime(csvListValues[i][2], provider)
                                    };

                                    list.Add(newItem);
                                    id++;
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private decimal GetOperatorValue(string strValue)
        {
            decimal result = 0M;

            if (string.IsNullOrWhiteSpace(strValue))
                return result;

            if (strValue.IndexOf(",") < 0)
                return Convert.ToDecimal(strValue);

            result = Convert.ToDecimal(strValue.Replace(",", "."));

            return result;
        }
    }
}
