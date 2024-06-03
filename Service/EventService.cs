using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    /// <summary>
    /// Represents all actions with <see cref="Event"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IEventService 
    {
        /// <summary>
        /// Retrieves an event by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the event.</returns>
        Task<Event> GetByID(long id);
        /// <summary>
        /// Retrieves a collection of all events.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of events.</returns>
        Task<IEnumerable<Event>> GetEvents();
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="model">The data transfer object containing event details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddEventDto model);
        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="id">The identifier of the event to update.</param>
        /// <param name="model">The data transfer object containing updated event details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditEventDto model);
        /// <summary>
        /// Deletes an event by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }
    /// <summary>
    /// Represents the default implementation of the <see cref="IEventService"/>.
    /// </summary>
    /// <inheritdoc cref="IEventService"/>
    public class EventService : IEventService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work. <see cref="IUnitOfWork"/> </param>
        /// <param name="mapper">The mapper.</param>
        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <inheritdoc/>
        public async Task Create(AddEventDto model)
        {
            await _unitOfWork.EventRepository.Insert( _mapper.Map<Event>(model) );
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.EventRepository.Delete(id);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public Task<Event> GetByID(long id)
        {
            return _unitOfWork.EventRepository.GetByID(id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<Event>> GetEvents()
        {
            return _unitOfWork.EventRepository.Get();
        }
        /// <inheritdoc/>
        public async Task Update(long id, EditEventDto model)
        {
            var item = await _unitOfWork.EventRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No Event to update");
            _mapper.Map(model, item);
            _unitOfWork.EventRepository.Update(item);
            await _unitOfWork.Save();
        }
    }
}
