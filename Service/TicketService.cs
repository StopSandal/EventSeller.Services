using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Ticket;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ITicketService"/>.
    /// </summary>
    /// <inheritdoc cref="ITicketService"/>
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task Create(AddTicketDto model)
        {
            await _unitOfWork.TicketRepository.Insert(_mapper.Map<Ticket>(model));
            await _unitOfWork.Save();
        }

        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.TicketRepository.Delete(id);
            await _unitOfWork.Save();
        }

        /// <inheritdoc/>
        public Task<Ticket> GetByID(long id)
        {
            return _unitOfWork.TicketRepository.GetByID(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Ticket>> GetTickets()
        {
            return _unitOfWork.TicketRepository.Get();
        }

        /// <inheritdoc/>
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
