using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.HallSector;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IHallSectorService"/>.
    /// </summary>
    /// <inheritdoc cref="IHallSectorService"/>
    public class HallSectorService : IHallSectorService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="HallSectorService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper./></param>
        public HallSectorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <inheritdoc/>
        public async Task CreateAsync(AddHallSectorDto model)
        {
            var item = _mapper.Map<HallSector>(model);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            await _unitOfWork.HallSectorRepository.InsertAsync(item);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.HallSectorRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public Task<HallSector> GetByIDAsync(long id)
        {
            return _unitOfWork.HallSectorRepository.GetByIDAsync(id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<HallSector>> GetHallSectorsAsync()
        {
            return _unitOfWork.HallSectorRepository.GetAsync();
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditHallSectorDto model)
        {
            var item = await _unitOfWork.HallSectorRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No HallSector to update");
            _mapper.Map(model, item);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            _unitOfWork.HallSectorRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Validates that the hall sector has unique fields before performing any database operations.
        /// </summary>
        /// <param name="model">The hall sector model to validate.</param>
        /// <param name="errorMessage">The error message to throw if validation fails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a duplicate hall sector is found.</exception>
        private async Task ValidateUniqueFields(HallSector model, string errorMessage)
        {
            if ((await _unitOfWork.HallSectorRepository.GetAsync(x => x.SectorName == model.SectorName && x.PlaceHallID == x.PlaceHallID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
