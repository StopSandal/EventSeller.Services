
namespace EventSeller.Services.Interfaces.Exporters
{
    public interface IFileExport
    {
        Task<Stream> ExportFileAsync<TClass>(TClass statistics);
    }
}
