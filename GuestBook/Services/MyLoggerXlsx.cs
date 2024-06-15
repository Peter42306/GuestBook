using OfficeOpenXml;
using System.ComponentModel;

namespace GuestBook.Services
{
    public class MyLoggerXlsx : IMyLogger
    {
        private readonly string _filePath;

        public MyLoggerXlsx(string filePath)
        {
            _filePath = filePath;
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            

            // Создаем файл и заголовки, если файл не существует
            if (!File.Exists(filePath))
            {
                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("MyExcelLogger"); // Добавляем лист с названием "MyExcelLogger"
                    workSheet.Cells[1, 1].Value = "DateTime"; // Устанавливаем заголовки для колонок
                    workSheet.Cells[1, 2].Value = "Message";
                    package.SaveAs(new FileInfo(_filePath)); // Сохраняем пакет как новый файл Excel по указанному пути
                }
            }
        }

        public void Log(string message)
        {
            // Открываем существующий файл Excel или создаем новый, если файл не существует
            using (var package = new ExcelPackage(new FileInfo(_filePath)))
            {
                var workSheet = package.Workbook.Worksheets["MyExcelLogger"]; // Получаем лист "MyExcelLogger" из открытого пакета Excel
                int rowCount = 1; // Проверяем, есть ли данные на листе

                // Проверяем, есть ли данные на листе
                if (workSheet.Dimension != null)
                {
                    rowCount = workSheet.Dimension.Rows; // Получаем количество строк с данными на листе
                }

                workSheet.Cells[rowCount + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Записываем текущую дату и время в колонку 1, следующую за последней заполненной строкой
                workSheet.Cells[rowCount + 1, 2].Value = message; // Записываем сообщение в колонку 2, следующую за последней заполненной строкой
                package.Save(); // Сохраняем изменения в файл Excel
            }
        }
    }
}
