using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.TicketSeat;

namespace Services.Service
{
    public interface ITicketSeatService
    {
        TicketSeat GetByID(long id);
        IEnumerable<TicketSeat> GetTicketSeats();
        void Create(AddTicketSeatDto model);
        void Update(long id, EditTicketSeatDto model);
        void Delete(long id);
    }

    public class TicketSeatService : ITicketSeatService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public TicketSeatService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Create(AddTicketSeatDto model)
        {
            _unitOfWork.TicketSeatRepository.Insert(_mapper.Map<TicketSeat>(model));
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.TicketSeatRepository.Delete(id);
            _unitOfWork.Save();
        }

        public TicketSeat GetByID(long id)
        {
            return _unitOfWork.TicketSeatRepository.GetByID(id);
        }

        public IEnumerable<TicketSeat> GetTicketSeats()
        {
            return _unitOfWork.TicketSeatRepository.Get();
        }

        public void Update(long id, EditTicketSeatDto model)
        {
            var item = _unitOfWork.TicketSeatRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No TicketSeat to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketSeatRepository.Update(item);
            _unitOfWork.Save();
        }
    }
}
