using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Ticket;

namespace Services.Service
{
    public interface ITicketService
    {
        Ticket GetByID(long id);
        IEnumerable<Ticket> GetTickets();
        void Create(AddTicketDto model);
        void Update(long id, EditTicketDto model);
        void Delete(long id);
    }

    public class TicketService : ITicketService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public TicketService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Create(AddTicketDto model)
        {
            _unitOfWork.TicketRepository.Insert(_mapper.Map<Ticket>(model));
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.TicketRepository.Delete(id);
            _unitOfWork.Save();
        }

        public Ticket GetByID(long id)
        {
            return _unitOfWork.TicketRepository.GetByID(id);
        }

        public IEnumerable<Ticket> GetTickets()
        {
            return _unitOfWork.TicketRepository.Get();
        }

        public void Update(long id, EditTicketDto model)
        {
            var item = _unitOfWork.TicketRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No Ticket to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketRepository.Update(item);
            _unitOfWork.Save();
        }
    }
}
