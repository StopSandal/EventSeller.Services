
using Microsoft.AspNetCore.Mvc;

namespace EventSeller.Services.Interfaces.Exporters
{
    public interface IResultExportService
    {
        public Task<IActionResult> ExportDataAsync<T>(T data, string format, string fileName);
    }
}
