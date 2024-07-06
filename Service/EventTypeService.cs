using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.EventType;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class EventTypeService : IEventTypeService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public EventTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateAsync(AddEventTypeDTO model)
        {
            await _unitOfWork.EventTypeRepository.InsertAsync(_mapper.Map<EventType>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.EventTypeRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EventType> GetByIDAsync(long id)
        {
            return await _unitOfWork.EventTypeRepository.GetByIDAsync(id);
        }

        public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            return await _unitOfWork.EventTypeRepository.GetAsync();
        }

        public async Task UpdateAsync(long id, EditEventTypeDTO model)
        {
            var item = await _unitOfWork.EventTypeRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No EventType to update");
            _mapper.Map(model, item);
            _unitOfWork.EventTypeRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
    }
}
