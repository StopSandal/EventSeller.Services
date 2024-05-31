using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceAddress;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    public interface IPlaceAddressService
    {
        Task<PlaceAddress> GetByID(long id);
        Task<IEnumerable<PlaceAddress>> GetPlaceAddresses();
        Task Create(AddPlaceAddressDto model);
        Task Update(long id, EditPlaceAddressDto model);
        Task Delete(long id);
    }

    public class PlaceAddressService : IPlaceAddressService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public PlaceAddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(AddPlaceAddressDto model)
        {
            await _unitOfWork.PlaceAddressRepository.Insert(_mapper.Map<PlaceAddress>(model));
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.PlaceAddressRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<PlaceAddress> GetByID(long id)
        {
            return _unitOfWork.PlaceAddressRepository.GetByID(id);
        }

        public Task<IEnumerable<PlaceAddress>> GetPlaceAddresses()
        {
            return _unitOfWork.PlaceAddressRepository.Get();
        }

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
