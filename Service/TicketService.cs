using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Ticket;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ITicketService"/>.
    /// </summary>
    /// <inheritdoc cref="ITicketService"/>
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddTicketDto model)
        {
            await _unitOfWork.TicketRepository.InsertAsync(_mapper.Map<Ticket>(model));
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task AddTicketListAsync(IEnumerable<Ticket> ticketList)
        {
            await _unitOfWork.TicketRepository.InsertRangeAsync(ticketList);
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.TicketRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public Task<Ticket> GetByIDAsync(long id)
        {
            return _unitOfWork.TicketRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return _unitOfWork.TicketRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditTicketDto model)
        {
            var item = await _unitOfWork.TicketRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No Ticket to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        public async Task<Ticket> GetTicketWithAllIncudesByIdAsync(long ticketId)
        {
            string includeProperties = "Event,Event.EventType";

            var ticket = await _unitOfWork.TicketRepository.GetAsync(
                filter: t => t.ID == ticketId,
                includeProperties: includeProperties
            );

            return ticket.FirstOrDefault();
        }

        public async Task<DateTime?> GetTicketEventStartDateTimeByIdAsync(long ticketId)
        {
            string includeProperties = "Event";

            var ticketList = await _unitOfWork.TicketRepository.GetAsync(
                filter: t => t.ID == ticketId,
                includeProperties: includeProperties
            );
            var ticket = ticketList.FirstOrDefault();
            if (ticket.Event == null)
            {
                throw new InvalidOperationException("Ticket is corrupted");
            }
            return ticket.TicketStartDateTime ?? ticket.Event.StartEventDateTime;
        }
    }
}
