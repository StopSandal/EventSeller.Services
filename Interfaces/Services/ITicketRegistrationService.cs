using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Ticket;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ITicketRegistrationService
    {
        // all seats at hall with constraints( for circus with rows start and end )
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync(AddTicketsForHallToFillDTO addTicketsForHallToFillDTO);
        //directly for hall and count
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync(AddTicketsForHallByCountDTO addTicketsForHallByCountDTO);

    }
}
