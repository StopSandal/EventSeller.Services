using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.PlaceHall;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPlaceHallService"/>.
    /// </summary>
    /// <inheritdoc cref="IPlaceHallService"/>
    public class PlaceHallService : IPlaceHallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaceHallService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceHallService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public PlaceHallService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlaceHallService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddPlaceHallDto model)
        {
            _logger.LogInformation("Creating new place hall.");
            var item = _mapper.Map<PlaceHall>(model);
            await ValidateUniqueFieldsAsync(item, "There is already existing same PlaceHallName for PlaceAddress");
            await _unitOfWork.PlaceHallRepository.InsertAsync(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place hall created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting place hall with ID: {Id}", id);
            await _unitOfWork.PlaceHallRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place hall deleted successfully.");
        }

        /// <inheritdoc/>
        public Task<PlaceHall> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching place hall by ID: {Id}", id);
            return _unitOfWork.PlaceHallRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public async Task<bool> DoesExistsByIdAsync(long id)
        {
            _logger.LogInformation("Checking if place hall exists by ID: {Id}", id);
            return await _unitOfWork.PlaceHallRepository.DoesExistsAsync(obj => obj.ID == id);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<PlaceHall>> GetPlaceHallsAsync()
        {
            _logger.LogInformation("Fetching all place halls.");
            return _unitOfWork.PlaceHallRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditPlaceHallDto model)
        {
            _logger.LogInformation("Updating place hall with ID: {Id}", id);
            var item = await _unitOfWork.PlaceHallRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Place hall with ID {Id} not found.", id);
                throw new NullReferenceException($"Place hall with ID {id} not found.");
            }

            _mapper.Map(model, item);
            await ValidateUniqueFieldsAsync(item, "There is already existing same PlaceHallName for PlaceAddress");
            _unitOfWork.PlaceHallRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place hall updated successfully.");
        }

        /// <summary>
        /// Validates that the place hall has unique fields before performing any database operations.
        /// </summary>
        /// <param name="model">The place hall model to validate.</param>
        /// <param name="errorMessage">The error message to throw if validation fails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a duplicate place hall is found.</exception>
        private async Task ValidateUniqueFieldsAsync(PlaceHall model, string errorMessage)
        {
            if ((await _unitOfWork.PlaceHallRepository.GetAsync(x => x.HallName == model.HallName && x.PlaceAddressID == model.PlaceAddressID)).Any())
            {
                _logger.LogError("Duplicate place hall found: {ErrorMessage}", errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TicketSeat>> GetAllSeatsInRangeByIdAsync(long placeHallId, int minRow, int maxRow)
        {
            _logger.LogInformation("Fetching all seats in range for place hall ID: {Id}", placeHallId);
            Expression<Func<TicketSeat, bool>> filter = ts => ts.HallSector.PlaceHallID == placeHallId && ts.PlaceRow >= minRow && ts.PlaceRow <= maxRow;
            return await _unitOfWork.TicketSeatRepository.GetAsync(filter, null, "HallSector");
        }
    }
}