using DataLayer.Model;
using DataLayer.Models.PlaceAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with the <see cref="PlaceAddress"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IPlaceAddressService
    {
        /// <summary>
        /// Retrieves a place address by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the place address.</returns>
        Task<PlaceAddress> GetByID(long id);

        /// <summary>
        /// Retrieves a collection of all place addresses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of place addresses.</returns>
        Task<IEnumerable<PlaceAddress>> GetPlaceAddresses();

        /// <summary>
        /// Creates a new place address.
        /// </summary>
        /// <param name="model">The data transfer object containing place address details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddPlaceAddressDto model);

        /// <summary>
        /// Updates an existing place address.
        /// </summary>
        /// <param name="id">The identifier of the place address to update.</param>
        /// <param name="model">The data transfer object containing updated place address details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditPlaceAddressDto model);

        /// <summary>
        /// Deletes a place address by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place address to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }
}
