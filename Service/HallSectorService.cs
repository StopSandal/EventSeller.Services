using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.HallSector;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IHallSectorService"/>.
    /// </summary>
    /// <inheritdoc cref="IHallSectorService"/>
    public class HallSectorService : IHallSectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HallSectorService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HallSectorService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public HallSectorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HallSectorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddHallSectorDto model)
        {
            _logger.LogInformation("Creating new hall sector.");
            var item = _mapper.Map<HallSector>(model);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            await _unitOfWork.HallSectorRepository.InsertAsync(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Hall sector created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting hall sector with ID: {Id}", id);
            await _unitOfWork.HallSectorRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Hall sector deleted successfully.");
        }

        /// <inheritdoc/>
        public Task<HallSector> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching hall sector by ID: {Id}", id);
            return _unitOfWork.HallSectorRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<HallSector>> GetHallSectorsAsync()
        {
            _logger.LogInformation("Fetching all hall sectors.");
            return _unitOfWork.HallSectorRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditHallSectorDto model)
        {
            _logger.LogInformation("Updating hall sector with ID: {Id}", id);
            var item = await _unitOfWork.HallSectorRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Hall sector with ID {Id} not found.", id);
                throw new NullReferenceException($"Hall sector with ID {id} not found.");
            }

            _mapper.Map(model, item);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            _unitOfWork.HallSectorRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Hall sector updated successfully.");
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
            if ((await _unitOfWork.HallSectorRepository.GetAsync(x => x.SectorName == model.SectorName && x.PlaceHallID == model.PlaceHallID)).Any())
            {
                _logger.LogError("Duplicate hall sector found: {ErrorMessage}", errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}