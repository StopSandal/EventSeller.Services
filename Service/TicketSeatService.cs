using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.TicketSeat;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ITicketSeatService"/>.
    /// </summary>
    /// <inheritdoc cref="ITicketSeatService"/>
    public class TicketSeatService : ITicketSeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketSeatService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSeatService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public TicketSeatService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketSeatService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddTicketSeatDto model)
        {
            _logger.LogInformation("Creating new ticket seat.");
            await _unitOfWork.TicketSeatRepository.InsertAsync(_mapper.Map<TicketSeat>(model));
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket seat created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting ticket seat with ID: {Id}", id);
            await _unitOfWork.TicketSeatRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket seat deleted successfully.");
        }

        /// <inheritdoc/>
        public Task<TicketSeat> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching ticket seat by ID: {Id}", id);
            return _unitOfWork.TicketSeatRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TicketSeat>> GetTicketSeatsAsync()
        {
            _logger.LogInformation("Fetching all ticket seats.");
            return _unitOfWork.TicketSeatRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditTicketSeatDto model)
        {
            _logger.LogInformation("Updating ticket seat with ID: {Id}", id);
            var item = await _unitOfWork.TicketSeatRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Ticket seat with ID {Id} not found.", id);
                throw new NullReferenceException($"Ticket seat with ID {id} not found.");
            }

            _mapper.Map(model, item);
            _unitOfWork.TicketSeatRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Ticket seat updated successfully.");
        }
    }
}