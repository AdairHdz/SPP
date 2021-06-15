using DataPersistenceLayer.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Utilities
{
    public class IndicatorsReportGenerator
    {
        private List<ReportData> _reportDataList = new List<ReportData>();
        public IndicatorsReportGenerator(List<Assignment> assignments, List<Sector> sectors)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            foreach (var sector in sectors)
            {
                ReportData reportData = new ReportData();
                reportData.SectorName = sector.NameSector;
                foreach(var assignment in assignments)
                {
                    if (assignment.Project.LinkedOrganization.Sector.NameSector.Equals(sector.NameSector))
                    {
                        StudentData studentData = new StudentData();
                        studentData.Gender = assignment.Practicioner.User.Gender;
                        reportData.Students.Add(studentData);
                    }
                }
                _reportDataList.Add(reportData);
            }
        }

        public async Task SaveExcelFile(FileInfo file)
        {
            DeleteIfExists(file);
            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("MainReport");
                var range = workSheet.Cells["A1:C10"];
                range.AutoFitColumns();
                workSheet.Column(1).Width = 30;
                workSheet.Cells["A1"].Value = "Información del ciclo escolar";
                workSheet.Cells["A1:E1"].Merge = true;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Style.Font.Size = 12;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells["A3"].Value = "Región";
                workSheet.Cells["A4"].Value = "Área académica";
                workSheet.Cells["A5"].Value = "Modalidad";
                workSheet.Cells["A6"].Value = "Nivel";
                workSheet.Cells["A7"].Value = "Programa educativo";
                workSheet.Cells["A8"].Value = "ServiciosS.S. Pract. Prof. e Int. Acad.";
                workSheet.Cells["A8:C8"].Merge = true;


                workSheet.Cells["B3"].Value = "XALAPA";
                workSheet.Cells["B4"].Value = "ECONÓMICO ADMINISTRATIVA";
                workSheet.Cells["B5"].Value = "ESCOLARIZADO";
                workSheet.Cells["B6"].Value = "LICENCIATURA";
                workSheet.Cells["B7"].Value = "INGENIERÍA DE SOFTWARE";

                workSheet.Cells["A10"].Value = "Número de alumnos que terminaron en el ciclo escolar anterior y que realizaron prácticas profesionales, por sexo";
                workSheet.Cells["A10:J10"].Merge = true;

                workSheet.Cells["A12"].Value = "Sector";
                workSheet.Cells["C12"].Value = "Hombres";
                workSheet.Cells["E12"].Value = "Mujeres";
                workSheet.Cells["G12"].Value = "Total";

                workSheet.Cells["A12:G12"].Style.Font.Bold = true;
                workSheet.Cells["A12:G12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells["A17"].Value = "Total";

                int currentCell = 13;
                int totalMen = 0;
                int totalWomen = 0;
                for (int i = 0; i < _reportDataList.Count; i++)
                {
                    workSheet.Cells[$"A{currentCell + i}"].Value = _reportDataList[i].SectorName;
                    int men = 0;
                    int women = 0;
                    foreach (var student in _reportDataList[i].Students)
                    {
                        if (student.Gender == Gender.MALE) //Hombre
                        {
                            totalMen++;
                            men++;
                        }
                        else
                        {
                            totalWomen++;
                            women++;
                        }
                    }
                    workSheet.Cells[$"C{currentCell + i}"].Value = men;
                    workSheet.Cells[$"E{currentCell + i}"].Value = women;
                    workSheet.Cells[$"G{currentCell + i}"].Value = women + men;
                }

                workSheet.Cells["C17"].Value = totalMen;
                workSheet.Cells["E17"].Value = totalWomen;
                workSheet.Cells["G17"].Value = totalWomen + totalMen;

                await package.SaveAsync();
            }
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }


    }
}
