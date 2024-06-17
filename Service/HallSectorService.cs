using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.HallSector;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
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
        public async Task Create(AddHallSectorDto model)
        {
            var item = _mapper.Map<HallSector>(model);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            await _unitOfWork.HallSectorRepository.Insert(item);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.HallSectorRepository.Delete(id);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public Task<HallSector> GetByID(long id)
        {
            return _unitOfWork.HallSectorRepository.GetByID(id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<HallSector>> GetHallSectors()
        {
            return _unitOfWork.HallSectorRepository.Get();
        }
        /// <inheritdoc/>
        public async Task Update(long id, EditHallSectorDto model)
        {
            var item = await _unitOfWork.HallSectorRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No HallSector to update");
            _mapper.Map(model, item);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            _unitOfWork.HallSectorRepository.Update(item);
            await _unitOfWork.Save();
        }
        /// <summary>
        /// Validates that the hall sector has unique fields before performing any database operations.
        /// </summary>
        /// <param name="model">The hall sector model to validate.</param>
        /// <param name="errorMessage">The error message to throw if validation fails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a duplicate hall sector is found.</exception>
        private async Task ValidateUniqueFields(HallSector model,string errorMessage)
        {
            if ((await _unitOfWork.HallSectorRepository.Get(x => x.SectorName == model.SectorName && x.PlaceHallID == x.PlaceHallID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
