using EventSeller.Services.Helpers.Constants;
using EventSeller.Services.Interfaces.Exporters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EventSeller.Services.Helpers
{
    public class ResultExportService : IResultExportService
    {
        private readonly IExcelFileExport _excelFileExport;
        private readonly ICsvFileExport _csvFileExport;
        private readonly ILogger<ResultExportService> _logger;

        public ResultExportService(IExcelFileExport excelFileExport, ICsvFileExport csvFileExport, ILogger<ResultExportService> logger)
        {
            _excelFileExport = excelFileExport;
            _csvFileExport = csvFileExport;
            _logger = logger;
        }

        public async Task<IActionResult> ExportDataAsync<T>(T data, string format, string fileName)
        {
            switch (format.ToLower())
            {
                case ExportConstants.ExcelTypeFormat:
                    var excelStream = await _excelFileExport.ExportFileAsync(data);
                    return new FileStreamResult(excelStream, ExportConstants.ExcelContentType)
                    {
                        FileDownloadName = $"{fileName}{ExportConstants.ExcelExtension}"
                    };

                case ExportConstants.CsvTypeFormat:
                    var csvStream = await _csvFileExport.ExportFileAsync(data);
                    return new FileStreamResult(csvStream, ExportConstants.CsvContentType)
                    {
                        FileDownloadName = $"{fileName}{ExportConstants.CsvExtension}"
                    };

                case ExportConstants.DefaultTypeFormat:
                    return new OkObjectResult(data);

                default:
                    return new BadRequestObjectResult($"Invalid format specified. Supported formats are '{ExportConstants.DefaultTypeFormat}', '{ExportConstants.CsvTypeFormat}', and '{ExportConstants.ExcelTypeFormat}'.");
            }
        }
    }
}
