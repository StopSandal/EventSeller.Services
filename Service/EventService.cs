using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Event;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IEventService"/>.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddEventDto model)
        {
            _logger.LogInformation("Creating new event.");
            var entity = _mapper.Map<Event>(model);
            await _unitOfWork.EventRepository.InsertAsync(entity);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting event with ID: {Id}", id);
            await _unitOfWork.EventRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event deleted successfully.");
        }

        /// <inheritdoc/>
        public Task<Event> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching event by ID: {Id}", id);
            return _unitOfWork.EventRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Event?> GetWithIncludesByIDAsync(long id, string includeProperties = null)
        {
            _logger.LogInformation("Fetching event with includes by ID: {Id}", id);
            var events = await _unitOfWork.EventRepository.GetAsync(obj => obj.ID == id, null, includeProperties);
            return events.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Event>> GetEventsAsync()
        {
            _logger.LogInformation("Fetching all events.");
            return _unitOfWork.EventRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditEventDto model)
        {
            _logger.LogInformation("Updating event with ID: {Id}", id);
            var existingEvent = await _unitOfWork.EventRepository.GetByIDAsync(id);
            if (existingEvent == null)
            {
                _logger.LogError("Event with ID {Id} not found.", id);
                throw new NullReferenceException($"Event with ID {id} not found.");
            }

            _mapper.Map(model, existingEvent);
            _unitOfWork.EventRepository.Update(existingEvent);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Event updated successfully.");
        }

        /// <inheritdoc/>
        public async Task<bool> DoesExistsByIdAsync(long id)
        {
            _logger.LogInformation("Checking if event exists by ID: {Id}", id);
            return await _unitOfWork.EventRepository.DoesExistsAsync(obj => obj.ID == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<Event, bool>> filter, Expression<Func<Event, TField>> selector)
        {
            _logger.LogInformation("Fetching field values for events with filter.");
            return await _unitOfWork.EventRepository.GetFieldValuesAsync(filter, selector);
        }
    }
}
