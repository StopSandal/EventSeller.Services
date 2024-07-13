using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.EventSession;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IEventSessionService"/>.
    /// </summary>
    public class EventSessionService : IEventSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EventSessionService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSessionService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public EventSessionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventSessionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<EventSession> CreateAsync(AddEventSessionDTO model)
        {
            _logger.LogInformation("Creating event session.");

            var eventSession = _mapper.Map<EventSession>(model);
            await _unitOfWork.EventSessionRepository.InsertAsync(eventSession);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Event session created successfully.");

            return eventSession;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting event session with ID: {EventSessionId}", id);

            await _unitOfWork.EventSessionRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Event session deleted successfully.");
        }

        /// <inheritdoc/>
        public async Task<EventSession> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching event session by ID: {EventSessionId}", id);

            var eventSession = await _unitOfWork.EventSessionRepository.GetByIDAsync(id);

            if (eventSession == null)
            {
                _logger.LogWarning("Event session with ID {EventSessionId} not found.", id);
            }

            _logger.LogInformation("Event session fetched successfully.");

            return eventSession;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventSession>> GetEventSessionsAsync()
        {
            _logger.LogInformation("Fetching all event sessions.");

            var eventSessions = await _unitOfWork.EventSessionRepository.GetAsync();

            _logger.LogInformation("Fetched {Count} event sessions.", eventSessions.Count());

            return eventSessions;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditEventSessionDTO model)
        {
            _logger.LogInformation("Updating event session with ID: {EventSessionId}", id);

            var eventSession = await _unitOfWork.EventSessionRepository.GetByIDAsync(id);

            if (eventSession == null)
            {
                _logger.LogWarning("Event session with ID {EventSessionId} not found. Update operation aborted.", id);
                throw new NullReferenceException($"Event session with ID {id} not found.");
            }

            _mapper.Map(model, eventSession);
            _unitOfWork.EventSessionRepository.Update(eventSession);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Event session updated successfully.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<EventSession, bool>> filter, Expression<Func<EventSession, TField>> selector)
        {
            _logger.LogInformation("Fetching field values for event sessions.");

            var fieldValues = await _unitOfWork.EventSessionRepository.GetFieldValuesAsync(filter, selector);

            _logger.LogInformation("Fetched {Count} field values for event sessions.", fieldValues.Count());

            return fieldValues;
        }
    }
}
