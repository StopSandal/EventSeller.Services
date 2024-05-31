using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;

namespace Services.Service
{
    public interface IEventService 
    {
        Event GetByID(long id);
        IEnumerable<Event> GetEvents();
        void Create(AddEventDto model);
        void Update(long id, EditEventDto model);
        void Delete(long id);
    }

    public class EventService : IEventService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public EventService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Create(AddEventDto model)
        {
            _unitOfWork.EventRepository.Insert( _mapper.Map<Event>(model) );
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.EventRepository.Delete(id);
            _unitOfWork.Save();
        }

        public Event GetByID(long id)
        {
            return _unitOfWork.EventRepository.GetByID(id);
        }

        public IEnumerable<Event> GetEvents()
        {
            return _unitOfWork.EventRepository.Get();
        }

        public void Update(long id, EditEventDto model)
        {
            var item = _unitOfWork.EventRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No Event to update");
            _mapper.Map(model, item);
            _unitOfWork.EventRepository.Update(item);
            _unitOfWork.Save();
        }
    }
}
