using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    public interface IEventService 
    {
        Task<Event> GetByID(long id);
        Task<IEnumerable<Event>> GetEvents();
        Task Create(AddEventDto model);
        Task Update(long id, EditEventDto model);
        Task Delete(long id);
    }

    public class EventService : IEventService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(AddEventDto model)
        {
            await _unitOfWork.EventRepository.Insert( _mapper.Map<Event>(model) );
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.EventRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<Event> GetByID(long id)
        {
            return _unitOfWork.EventRepository.GetByID(id);
        }

        public  Task<IEnumerable<Event>> GetEvents()
        {
            return _unitOfWork.EventRepository.Get();
        }

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
