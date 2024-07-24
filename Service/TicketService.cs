using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Ticket;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ITicketService"/>.
    /// </summary>
    /// <inheritdoc cref="ITicketService"/>
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddTicketDto model)
        {
            _logger.LogInformation("Creating a new ticket.");
            await _unitOfWork.TicketRepository.InsertAsync(_mapper.Map<Ticket>(model));
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket created successfully.");
        }

        /// <inheritdoc/>
        public async Task AddTicketListAsync(IEnumerable<Ticket> ticketList)
        {
            _logger.LogInformation("Adding a list of tickets.");
            await _unitOfWork.TicketRepository.InsertRangeAsync(ticketList);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Tickets added successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting ticket with ID: {Id}", id);
            await _unitOfWork.TicketRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket deleted successfully.");
        }

        /// <inheritdoc/>
        public Task<Ticket> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching ticket by ID: {Id}", id);
            return _unitOfWork.TicketRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            _logger.LogInformation("Fetching all tickets.");
            return _unitOfWork.TicketRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditTicketDto model)
        {
            _logger.LogInformation("Updating ticket with ID: {Id}", id);
            var item = await _unitOfWork.TicketRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Ticket with ID {Id} not found.", id);
                throw new NullReferenceException($"Ticket with ID {id} not found.");
            }

            _mapper.Map(model, item);
            _unitOfWork.TicketRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket updated successfully.");
        }

        /// <inheritdoc/>
        public async Task<Ticket> GetTicketWithIncudesByIdAsync(long ticketId, string includes)
        {
            _logger.LogInformation("Fetching ticket with includes by ID: {TicketId}", ticketId);
            var ticket = await _unitOfWork.TicketRepository.GetAsync(
                filter: t => t.ID == ticketId,
                includeProperties: includes
            );

            return ticket.FirstOrDefault();
        }
    }
}