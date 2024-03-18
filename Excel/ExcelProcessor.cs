﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.ObjectModel;

namespace AppliedJobsManager.Excel
{
    internal class ExcelProcessor
    {
        private readonly string _path;
        public ExcelProcessor(string path)
        {
            _path = path;
        }

        public ObservableCollection<Models.Row> Process()
        {
            var jobRows = new ObservableCollection<Models.Row>();

            using var spreadsheetDocument = SpreadsheetDocument.Open(_path, false);

            var workbookPart = spreadsheetDocument.WorkbookPart;
            var worksheetPart = workbookPart!.WorksheetParts.First();
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            foreach(var row in sheetData.Elements<Row>())
            {
                var cells = row.Elements<Cell>().ToList();

                if (cells.Count > 3)
                {
                    var jobRow = new Models.Row
                    {
                        Link = GetCellValue(spreadsheetDocument, cells[0]),
                        Description = GetCellValue(spreadsheetDocument, cells[1]),
                        Job = GetCellValue(spreadsheetDocument, cells[2]),
                        Pay = GetCellValue(spreadsheetDocument, cells[3]),
                        Date = GetCellValue(spreadsheetDocument, cells[4], true).Replace("-", "."),
                    };

                    jobRows.Add(jobRow);
                }
                 
            }

            spreadsheetDocument.Dispose();

            return jobRows;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell, bool isDate = false)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var value = cell.InnerText;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }

            if (isDate)
            {
                var dateTimeValue = DateTime.FromOADate(double.Parse(cell.InnerText));
                return dateTimeValue.ToString("yyyy-MM-dd");
            }

            return value;

        }
    }
}
