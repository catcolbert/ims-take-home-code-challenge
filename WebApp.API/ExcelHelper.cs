using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.API
{
    public class ExcelHelper
    {
        /// <summary>
        /// Read AdoptableDog import file
        /// </summary>
        /// <param name="fileName">Path of importing file</param>
        public static List<AdoptableDog> ReadAdoptionDogsSheetData(string fileName)
        {
            List<AdoptableDog> adoptableDogs = new List<AdoptableDog>();

            XLWorkbook workBook = new XLWorkbook(fileName);
            //Read the first Sheet from Excel file.
            IXLWorksheet workSheet = workBook.Worksheet("Sheet1"); //TODO: RAVI - Keep sheet name to config file
            if (workSheet == null)
                throw new Exception("Unable to read importing excel file.");

            //Loop through the Worksheet rows.                  
            int numberofRows = workSheet.Rows().Count();
            if (numberofRows < 2)
                throw new Exception("No data row found in importing excel file.");

            bool isHeader = true;
            int cellIndex = 0;
            AdoptableDog nDogInfo = null;
            foreach (IXLRow row in workSheet.Rows())
            {
                nDogInfo = new AdoptableDog();
                cellIndex = 0;
                //Use the first row to add columns to DataTable.
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                foreach (IXLCell cell in row.Cells())
                {
                    string value = cell.CachedValue.ToString();
                    switch (cellIndex)
                    {
                        case 0:
                            nDogInfo.Name = value;
                            break;
                        case 1:
                            nDogInfo.Location = value;
                            break;
                        case 2:
                            nDogInfo.Description = value;
                            break;
                        case 3:
                            DateTime dtAvailable = DateTime.ParseExact(value, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); //TODO: RAVI, Keep date stamp to config
                            nDogInfo.Date = dtAvailable;
                            break;
                    }
                    cellIndex++;
                }

                if (!string.IsNullOrEmpty(nDogInfo.Name))
                    adoptableDogs.Add(nDogInfo);

            }
            return adoptableDogs;
        }

    }
}
