using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Ticket;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Service interface for ticket registration operations.
    /// </summary>
    public interface ITicketRegistrationService
    {
        /// <summary>
        /// Adds tickets to fill all seats at all sectors for a specified place hall with event type options.
        /// </summary>
        /// <param name="addTicketsForHallToFillDTO">DTO containing information for adding tickets to fill a hall.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of added tickets.</returns>
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync(AddTicketsForHallToFillDTO addTicketsForHallToFillDTO);

        /// <summary>
        /// Adds tickets to a specified place hall based on the given count.
        /// </summary>
        /// <param name="addTicketsForHallByCountDTO">DTO containing information for adding tickets by count to a hall.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of added tickets.</returns>
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync(AddTicketsForHallByCountDTO addTicketsForHallByCountDTO);
    }
}
