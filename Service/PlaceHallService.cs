using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceHall;

namespace Services.Service
{
    public interface IPlaceHallService
    {
        Task<PlaceHall> GetByID(long id);
        Task<IEnumerable<PlaceHall>> GetPlaceHalls();
        Task Create(AddPlaceHallDto model);
        Task Update(long id, EditPlaceHallDto model);
        Task Delete(long id);
    }

    public class PlaceHallService : IPlaceHallService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public PlaceHallService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(AddPlaceHallDto model)
        {
            var item = _mapper.Map<PlaceHall>(model);
            await ValidateUniqueFields(item, "There is already existing same PlaceHallName for PlaceAddress");
            await _unitOfWork.PlaceHallRepository.Insert(item);
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.PlaceHallRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<PlaceHall> GetByID(long id)
        {
            return _unitOfWork.PlaceHallRepository.GetByID(id);
        }

        public Task<IEnumerable<PlaceHall>> GetPlaceHalls()
        {
            return _unitOfWork.PlaceHallRepository.Get();
        }

        public async Task Update(long id, EditPlaceHallDto model)
        {
            var item = await _unitOfWork.PlaceHallRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No PlaceHall to update");
            _mapper.Map(model, item);
            await ValidateUniqueFields(item, "There is already existing same PlaceHallName for PlaceAddress");
            _unitOfWork.PlaceHallRepository.Update(item);
            await _unitOfWork.Save();
        }
        private async Task ValidateUniqueFields(PlaceHall model, string errorMessage)
        {
            if ((await _unitOfWork.PlaceHallRepository.Get(x => x.HallName == model.HallName && x.PlaceAddressID == x.PlaceAddressID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
