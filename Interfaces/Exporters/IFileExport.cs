
namespace EventSeller.Services.Interfaces.Exporters
{
    /// <summary>
    /// Interface for exporting files asynchronously.
    /// </summary>
    public interface IFileExport
    {
        /// <summary>
        /// Exports data of type <typeparamref name="TClass"/> to a stream asynchronously.
        /// </summary>
        /// <typeparam name="TClass">The type of data to export.</typeparam>
        /// <param name="statistics">The data to export.</param>
        /// <returns>A task that represents the asynchronous export operation. The task result is the exported data as a stream.</returns>
        Task<Stream> ExportFileAsync<TClass>(TClass statistics);
    }
}
