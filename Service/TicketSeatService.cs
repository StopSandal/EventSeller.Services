using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.TicketSeat;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ITicketSeatService"/>.
    /// </summary>
    /// <inheritdoc cref="ITicketSeatService"/>
    public class TicketSeatService : ITicketSeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSeatService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public TicketSeatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddTicketSeatDto model)
        {
            await _unitOfWork.TicketSeatRepository.InsertAsync(_mapper.Map<TicketSeat>(model));
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.TicketSeatRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public Task<TicketSeat> GetByIDAsync(long id)
        {
            return _unitOfWork.TicketSeatRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TicketSeat>> GetTicketSeatsAsync()
        {
            return _unitOfWork.TicketSeatRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditTicketSeatDto model)
        {
            var item = await _unitOfWork.TicketSeatRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No TicketSeat to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketSeatRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
    }
}
