using AutoMapper;
using DataLayer.Model;
using DataLayer.Model.EF;
using DataLayer.Models.Ticket;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task CreateAsync(AddTicketDto model)
        {
            await _unitOfWork.TicketRepository.InsertAsync(_mapper.Map<Ticket>(model));
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task AddTicketListAsync(IEnumerable<Ticket> ticketList)
        {
            await _unitOfWork.TicketRepository.InsertRangeAsync(ticketList);
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.TicketRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        /// <inheritdoc/>
        public Task<Ticket> GetByIDAsync(long id)
        {
            return _unitOfWork.TicketRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return _unitOfWork.TicketRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditTicketDto model)
        {
            var item = await _unitOfWork.TicketRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No Ticket to update");
            _mapper.Map(model, item);
            _unitOfWork.TicketRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task<Ticket> GetTicketWithIncudesByIdAsync(long ticketId,string includes)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(
                filter: t => t.ID == ticketId,
                includeProperties: includes
            );

            return ticket.FirstOrDefault();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<Ticket, bool>> filter, Expression<Func<Ticket, TField>> selector)
        {
            return await _unitOfWork.TicketRepository.GetFieldValuesAsync(filter, selector);
        }
        /// <inheritdoc/>
        public async Task<int> GetTicketCountAsync(Expression<Func<Ticket,bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            return await _unitOfWork.TicketRepository.GetCountAsync(filter,includeProperties);
        }
        /// <inheritdoc/>
        public async Task<decimal> GetTicketAveragePriceAsync(Expression<Func<Ticket, bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            return await _unitOfWork.TicketRepository.GetAverageAsync(ticket => ticket.Price, filter);
        }
        /// <inheritdoc/>
        public async Task<decimal> GetTicketTotalPriceAsync(Expression<Func<Ticket, bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            return await _unitOfWork.TicketRepository.GetSumAsync(ticket => ticket.Price, filter);
        }
    }
}
