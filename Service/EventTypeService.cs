using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.EventType;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IEventTypeService"/>.
    /// </summary>
    public class EventTypeService : IEventTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EventTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public EventTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddEventTypeDTO model)
        {
            _logger.LogInformation("Creating new event type.");
            var entity = _mapper.Map<EventType>(model);
            await _unitOfWork.EventTypeRepository.InsertAsync(entity);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event type created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting event type with ID: {Id}", id);
            await _unitOfWork.EventTypeRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event type deleted successfully.");
        }

        /// <inheritdoc/>
        public async Task<EventType> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching event type by ID: {Id}", id);
            return await _unitOfWork.EventTypeRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            _logger.LogInformation("Fetching all event types.");
            return await _unitOfWork.EventTypeRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditEventTypeDTO model)
        {
            _logger.LogInformation("Updating event type with ID: {Id}", id);
            var item = await _unitOfWork.EventTypeRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Event type with ID {Id} not found.", id);
                throw new NullReferenceException($"Event type with ID {id} not found.");
            }

            _mapper.Map(model, item);
            _unitOfWork.EventTypeRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event type updated successfully.");
        }
    }
}