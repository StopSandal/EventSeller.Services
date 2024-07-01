using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
{
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
        public async Task CreateAsync(AddEventDto model)
        {
            await _unitOfWork.EventRepository.InsertAsync( _mapper.Map<Event>(model) );
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.EventRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public Task<Event> GetByIDAsync(long id)
        {
            return _unitOfWork.EventRepository.GetByIDAsync(id);
        }
        /// <inheritdoc/>
        public async Task<Event?> GetWithIncludesByIDAsync(long id,string includeProperties = null)
        {
            return (await _unitOfWork.EventRepository.GetAsync(obj => obj.ID==id , null , includeProperties)).FirstOrDefault();
        }
        /// <inheritdoc/>
        public Task<IEnumerable<Event>> GetEventsAsync()
        {
            return _unitOfWork.EventRepository.GetAsync();
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditEventDto model)
        {
            var item = await _unitOfWork.EventRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No Event to update");
            _mapper.Map(model, item);
            _unitOfWork.EventRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task<bool> DoesExistsByIdAsync(long id)
        {
            return await _unitOfWork.EventRepository.DoesExistsAsync(obj => obj.ID == id);
        }
    }
}
