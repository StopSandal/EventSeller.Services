using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.HallSector;

namespace Services.Service
{
    public interface IHallSectorService
    {
        HallSector GetByID(long id);
        IEnumerable<HallSector> GetHallSectors();
        void Create(AddHallSectorDto model);
        void Update(long id, EditHallSectorDto model);
        void Delete(long id);
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
        public void Create(AddHallSectorDto model)
        {
            var item = _mapper.Map<HallSector>(model);
            ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            _unitOfWork.HallSectorRepository.Insert(item);
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.HallSectorRepository.Delete(id);
            _unitOfWork.Save();
        }

        public HallSector GetByID(long id)
        {
            return _unitOfWork.HallSectorRepository.GetByID(id);
        }

        public IEnumerable<HallSector> GetHallSectors()
        {
            return _unitOfWork.HallSectorRepository.Get();
        }

        public void Update(long id, EditHallSectorDto model)
        {
            var item = _unitOfWork.HallSectorRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No HallSector to update");
            _mapper.Map(model, item);
            ValidateUniqueFields(item, "There is already existing same HallSectorName for PlaceHall");
            _unitOfWork.HallSectorRepository.Update(item);
            _unitOfWork.Save();
        }
        private void ValidateUniqueFields(HallSector model,string errorMessage)
        {
            if (_unitOfWork.HallSectorRepository.Get(x => x.SectorName == model.SectorName && x.PlaceHallID == x.PlaceHallID).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
