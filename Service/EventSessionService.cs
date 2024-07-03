using AutoMapper;
using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.EventSession;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class EventSessionService : IEventSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventSessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventSession> CreateAsync(AddEventSessionDTO model)
        {
            var eventSession = _mapper.Map<EventSession>(model);
            await _unitOfWork.EventSessionRepository.InsertAsync(eventSession);
            await _unitOfWork.SaveAsync();
            return eventSession;
        }

        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.EventSessionRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EventSession> GetByIDAsync(long id)
        {
            return await _unitOfWork.EventSessionRepository.GetByIDAsync(id);
        }

        public async Task<IEnumerable<EventSession>> GetEventSessionsAsync()
        {
            return await _unitOfWork.EventSessionRepository.GetAsync();
        }

        public async Task UpdateAsync(long id, EditEventSessionDTO model)
        { 
            var item = await _unitOfWork.EventSessionRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No Ticket to update");
            _mapper.Map(model, item);
            _unitOfWork.EventSessionRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<EventSession, bool>> filter, Expression<Func<EventSession, TField>> selector)
        {
            return await _unitOfWork.EventSessionRepository.GetFieldValuesAsync(filter, selector);
        }
    }
}
