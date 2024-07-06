using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.HallSector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with <see cref="HallSector"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IHallSectorService
    {
        /// <summary>
        /// Retrieves a hall sector by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the hall sector.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the hall sector.</returns>
        Task<HallSector> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all hall sectors.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sectors.</returns>
        Task<IEnumerable<HallSector>> GetHallSectorsAsync();

        /// <summary>
        /// Creates a new hall sector.
        /// </summary>
        /// <param name="model">The data transfer object containing hall sector details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddHallSectorDto model);

        /// <summary>
        /// Updates an existing hall sector.
        /// </summary>
        /// <param name="id">The identifier of the hall sector to update.</param>
        /// <param name="model">The data transfer object containing updated hall sector details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditHallSectorDto model);

        /// <summary>
        /// Deletes a hall sector by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the hall sector to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
    }
}
