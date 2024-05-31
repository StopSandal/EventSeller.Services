using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.TicketSeat;

namespace Services.Service
{
    public interface ITicketSeatService
    {
        Task<TicketSeat> GetByID(long id);
        Task<IEnumerable<TicketSeat>> GetTicketSeats();
        Task Create(AddTicketSeatDto model);
        Task Update(long id, EditTicketSeatDto model);
        Task Delete(long id);
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
        public async Task Create(AddTicketSeatDto model)
        {
            await _unitOfWork.TicketSeatRepository.Insert(_mapper.Map<TicketSeat>(model));
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.TicketSeatRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<TicketSeat> GetByID(long id)
        {
            return _unitOfWork.TicketSeatRepository.GetByID(id);
        }

        public Task<IEnumerable<TicketSeat>> GetTicketSeats()
        {
            return _unitOfWork.TicketSeatRepository.Get();
        }

        public async Task Update(long id, EditTicketSeatDto model)
        {
            var item = await _unitOfWork.TicketSeatRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No TicketSeat to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketSeatRepository.Update(item);
            await _unitOfWork.Save();
        }
    }
}
