using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Ticket;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    public interface ITicketService
    {
        Task<Ticket> GetByID(long id);
        Task<IEnumerable<Ticket>> GetTickets();
        Task Create(AddTicketDto model);
        Task Update(long id, EditTicketDto model);
        Task Delete(long id);
    }

    public class TicketService : ITicketService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(AddTicketDto model)
        {
            await _unitOfWork.TicketRepository.Insert(_mapper.Map<Ticket>(model));
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.TicketRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<Ticket> GetByID(long id)
        {
            return _unitOfWork.TicketRepository.GetByID(id);
        }

        public Task<IEnumerable<Ticket>> GetTickets()
        {
            return _unitOfWork.TicketRepository.Get();
        }

        public async Task Update(long id, EditTicketDto model)
        {
            var item = await _unitOfWork.TicketRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No Ticket to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketRepository.Update(item);
            await _unitOfWork.Save();
        }
    }
}
