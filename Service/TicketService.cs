using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Ticket;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    /// <summary>
    /// Represents all actions with the <see cref="Ticket"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface ITicketService
    {
        /// <summary>
        /// Retrieves a ticket by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ticket.</returns>
        Task<Ticket> GetByID(long id);

        /// <summary>
        /// Retrieves a collection of all tickets.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of tickets.</returns>
        Task<IEnumerable<Ticket>> GetTickets();

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="model">The data transfer object containing ticket details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddTicketDto model);

        /// <summary>
        /// Updates an existing ticket.
        /// </summary>
        /// <param name="id">The identifier of the ticket to update.</param>
        /// <param name="model">The data transfer object containing updated ticket details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditTicketDto model);

        /// <summary>
        /// Deletes a ticket by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }

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
