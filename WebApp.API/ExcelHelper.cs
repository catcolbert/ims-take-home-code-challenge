using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.API
{
    public sealed class ExcelHelper
    {
        /// <summary>
        /// The one and only Singleton instance. 
        /// </summary>
        private static ExcelHelper instance = null;
        private static readonly object padlock = new object();
        private string _excelDataFile = string.Empty;
        private string _excelFileName = string.Empty;
        private string _sheetName = string.Empty;
        private bool isInitialized = false;


        /// <summary>
        /// Private constructor
        /// </summary>
        private ExcelHelper()
        {
           
        }



        /// <summary>
        /// Gets the instance of the singleton object.
        /// </summary>
        public static ExcelHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ExcelHelper();
                    }
                    return instance;
                }
            }
        }

        public void Init(string _dbFile, string _sheet)
        {
            _excelFileName = _dbFile;
            _sheetName = _sheet;
            if (string.IsNullOrEmpty(_excelFileName))
                throw new FileNotFoundException("Excel file name not found in config.");
            if (string.IsNullOrEmpty(_sheetName))
                _sheetName = "Sheet1";

            DirectoryInfo dogsXlsDirInfo = VisualStudioProvider.TryGetSolutionDirectoryInfo();
            if (dogsXlsDirInfo != null)
                _excelDataFile = Path.Combine(dogsXlsDirInfo.FullName, _excelFileName);
        
            if(!File.Exists(_excelDataFile))
                throw new FileNotFoundException("Excel db file not found.");

        }
        /// <summary>
        /// Read AdoptableDog import file
        /// </summary>        
        public List<AdoptableDog> ReadAdoptionDogsSheetData()
        {
            if (isInitialized)
                throw new InvalidOperationException("Not intialized yet.");

            List<AdoptableDog> adoptableDogs = new List<AdoptableDog>();

            XLWorkbook workBook = new XLWorkbook(_excelDataFile);
            //Read the first Sheet from Excel file.
            IXLWorksheet workSheet = workBook.Worksheet(_sheetName); 
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

        /// <summary>
        /// Still in progress...
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public bool AddAdoptionDogsToSheet(AdoptableDog dog)
        {
            if (isInitialized)
                throw new InvalidOperationException("Not intialized yet.");

            List<AdoptableDog> adoptableDogs = new List<AdoptableDog>();

            XLWorkbook workBook = new XLWorkbook(_excelDataFile);
            //Read the first Sheet from Excel file.
            IXLWorksheet workSheet = workBook.Worksheet(_sheetName); 
            if (workSheet == null)
                throw new Exception("Unable to read importing excel file.");

            workSheet.Cell(workSheet.RowCount() + 1, 1).Value = "Test Dog" + DateTime.Now.ToString("HHmmss");
            workBook.Save();

            return true;
        }

    }
}
