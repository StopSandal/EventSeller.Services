using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.PlaceAddress;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPlaceAddressService"/>.
    /// </summary>
    /// <inheritdoc cref="IPlaceAddressService"/>
    public class PlaceAddressService : IPlaceAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaceAddressService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceAddressService"/> class with the specified unit of work, mapper, and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public PlaceAddressService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlaceAddressService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddPlaceAddressDto model)
        {
            _logger.LogInformation("Creating new place address.");
            await _unitOfWork.PlaceAddressRepository.InsertAsync(_mapper.Map<PlaceAddress>(model));
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place address created successfully.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting place address with ID: {Id}", id);
            await _unitOfWork.PlaceAddressRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place address deleted successfully.");
        }

        /// <inheritdoc/>
        public async Task<PlaceAddress> GetByIDAsync(long id)
        {
            _logger.LogInformation("Fetching place address by ID: {Id}", id);
            return await _unitOfWork.PlaceAddressRepository.GetByIDAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PlaceAddress>> GetPlaceAddressesAsync()
        {
            _logger.LogInformation("Fetching all place addresses.");
            return await _unitOfWork.PlaceAddressRepository.GetAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditPlaceAddressDto model)
        {
            _logger.LogInformation("Updating place address with ID: {Id}", id);
            var item = await _unitOfWork.PlaceAddressRepository.GetByIDAsync(id);
            if (item == null)
            {
                _logger.LogError("Place address with ID {Id} not found.", id);
                throw new NullReferenceException($"Place address with ID {id} not found.");
            }

            _mapper.Map(model, item);
            _unitOfWork.PlaceAddressRepository.Update(item);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Place address updated successfully.");
        }
    }
}