using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.PlaceAddress;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;

namespace Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPlaceAddressService"/>.
    /// </summary>
    /// <inheritdoc cref="IPlaceAddressService"/>
    public class PlaceAddressService : IPlaceAddressService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceAddressService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public PlaceAddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(AddPlaceAddressDto model)
        {
            await _unitOfWork.PlaceAddressRepository.InsertAsync(_mapper.Map<PlaceAddress>(model));
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.PlaceAddressRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task<PlaceAddress> GetByIDAsync(long id)
        {
            return await _unitOfWork.PlaceAddressRepository.GetByIDAsync(id);
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<PlaceAddress>> GetPlaceAddressesAsync()
        {
            return await _unitOfWork.PlaceAddressRepository.GetAsync();
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditPlaceAddressDto model)
        {
            var item = await _unitOfWork.PlaceAddressRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No PlaceAddress to update");
            _mapper.Map(model, item);
            _unitOfWork.PlaceAddressRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
    }
}
