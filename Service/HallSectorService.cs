using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.HallSector;

namespace Services.Service
{
    public interface IHallSectorService
    {
        Task<HallSector> GetByID(long id);
        Task<IEnumerable<HallSector>> GetHallSectors();
        Task Create(AddHallSectorDto model);
        Task Update(long id, EditHallSectorDto model);
        Task Delete(long id);
    }

    public class HallSectorService : IHallSectorService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public HallSectorService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(AddHallSectorDto model)
        {
            var item = _mapper.Map<HallSector>(model);
            await ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            await _unitOfWork.HallSectorRepository.Insert(item);
            await _unitOfWork.Save();
        }

        public async Task Delete(long id)
        {
            await _unitOfWork.HallSectorRepository.Delete(id);
            await _unitOfWork.Save();
        }

        public Task<HallSector> GetByID(long id)
        {
            return _unitOfWork.HallSectorRepository.GetByID(id);
        }

        public Task<IEnumerable<HallSector>> GetHallSectors()
        {
            return _unitOfWork.HallSectorRepository.Get();
        }

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
        private async Task ValidateUniqueFields(HallSector model,string errorMessage)
        {
            if ((await _unitOfWork.HallSectorRepository.Get(x => x.SectorName == model.SectorName && x.PlaceHallID == x.PlaceHallID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
