using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceAddress;
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
        public async Task Create(AddPlaceAddressDto model)
        {
            await _unitOfWork.PlaceAddressRepository.Insert(_mapper.Map<PlaceAddress>(model));
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.PlaceAddressRepository.Delete(id);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public Task<PlaceAddress> GetByID(long id)
        {
            return _unitOfWork.PlaceAddressRepository.GetByID(id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<PlaceAddress>> GetPlaceAddresses()
        {
            return _unitOfWork.PlaceAddressRepository.Get();
        }
        /// <inheritdoc/>
        public async Task Update(long id, EditPlaceAddressDto model)
        {
            var item = await _unitOfWork.PlaceAddressRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No PlaceAddress to update");
            _mapper.Map(model, item);
            _unitOfWork.PlaceAddressRepository.Update(item);
            await _unitOfWork.Save();
        }
    }
}
