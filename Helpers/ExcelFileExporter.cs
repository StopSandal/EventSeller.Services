using EventSeller.Services.Interfaces.Exporters;
using OfficeOpenXml;
using System.Collections;
using System.Reflection;

namespace EventSeller.Services.Helpers
{
    public class ExcelFileExporter : IExcelFileExport
    {
        private const string ExcelSheetName = "Statistics";
        public async Task<Stream> ExportFileAsync<TClass>(TClass statistics)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(ExcelSheetName);

            int currentRow = 1;
            int currentCol = 1;

            var properties = GetProperties(typeof(TClass));
            WriteHeader(properties, worksheet, ref currentRow, ref currentCol);
            currentRow++;

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TClass)) && typeof(TClass) != typeof(string))
            {
                foreach (var item in (IEnumerable)statistics)
                {
                    currentCol = 1;
                    WriteData(properties, worksheet, item, ref currentRow, ref currentCol);
                    currentRow++;
                }
            }
            else
            {
                currentCol = 1;
                WriteData(properties, worksheet, statistics, ref currentRow, ref currentCol);
                currentRow++;
            }

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            stream.Position = 0;
            return stream;
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                var elementType = type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();
                if (elementType != null)
                {
                    return elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                }
            }
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        private void WriteHeader(IEnumerable<PropertyInfo> properties, ExcelWorksheet worksheet, ref int currentRow, ref int currentCol)
        {
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
                {
                    var elementType = propertyType.IsGenericType ? propertyType.GetGenericArguments()[0] : propertyType.GetElementType();
                    if (elementType != null)
                    {
                        var elementProperties = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        currentRow++;
                        WriteHeader(elementProperties, worksheet, ref currentRow, ref currentCol);
                    }
                }
                else if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string) && property.PropertyType != typeof(decimal))
                {
                    var nestedProperties = property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    WriteHeader(nestedProperties, worksheet, ref currentRow, ref currentCol);
                }
                else
                {
                    worksheet.Cells[currentRow, currentCol].Value = property.Name;
                    currentCol++;
                }
            }
        }

        private void WriteData(IEnumerable<PropertyInfo> properties, ExcelWorksheet worksheet, object stat, ref int currentRow, ref int currentCol)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(stat);

                if (value is IEnumerable enumerable && !(value is string))
                {
                    foreach (var item in enumerable)
                    {
                        var itemProperties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        var tempColumn = currentCol;
                        WriteData(itemProperties, worksheet, item, ref currentRow, ref currentCol);
                        currentCol = tempColumn;
                        currentRow++;
                    }
                }
                else if (value != null && !value.GetType().IsPrimitive && value.GetType() != typeof(string) && value.GetType() != typeof(decimal))
                {
                    var nestedProperties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    WriteData(nestedProperties, worksheet, value, ref currentRow, ref currentCol);
                }
                else
                {
                    worksheet.Cells[currentRow, currentCol].Value = FormatValue(value);
                    currentCol++;
                }
            }
        }

        private object FormatValue(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return value ?? string.Empty;
        }
    }
}
