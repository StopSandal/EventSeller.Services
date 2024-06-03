using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.TicketSeat;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    /// <summary>
    /// Represents all actions with the <see cref="TicketSeat"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface ITicketSeatService
    {
        /// <summary>
        /// Retrieves a ticket seat by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ticket seat.</returns>
        Task<TicketSeat> GetByID(long id);

        /// <summary>
        /// Retrieves a collection of all ticket seats.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of ticket seats.</returns>
        Task<IEnumerable<TicketSeat>> GetTicketSeats();

        /// <summary>
        /// Creates a new ticket seat.
        /// </summary>
        /// <param name="model">The data transfer object containing ticket seat details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddTicketSeatDto model);

        /// <summary>
        /// Updates an existing ticket seat.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat to update.</param>
        /// <param name="model">The data transfer object containing updated ticket seat details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditTicketSeatDto model);

        /// <summary>
        /// Deletes a ticket seat by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }

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
        public async Task Create(AddTicketSeatDto model)
        {
            await _unitOfWork.TicketSeatRepository.Insert(_mapper.Map<TicketSeat>(model));
            await _unitOfWork.Save();
        }

        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.TicketSeatRepository.Delete(id);
            await _unitOfWork.Save();
        }

        /// <inheritdoc/>
        public Task<TicketSeat> GetByID(long id)
        {
            return _unitOfWork.TicketSeatRepository.GetByID(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TicketSeat>> GetTicketSeats()
        {
            return _unitOfWork.TicketSeatRepository.Get();
        }

        /// <inheritdoc/>
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
