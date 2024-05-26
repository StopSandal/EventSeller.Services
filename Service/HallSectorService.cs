using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.HallSector;

namespace Services.Service
{
    public interface IHallSectorService
    {
        HallSector GetByID(long id);
        IEnumerable<HallSector> GetHallSectors();
        void Create(CreateHallSector model);
        void Update(long id, UpdateHallSector model);
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
        public void Create(CreateHallSector model)
        {
            _unitOfWork.HallSectorRepository.Insert(_mapper.Map<HallSector>(model));
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

        public void Update(long id, UpdateHallSector model)
        {
            var item = _unitOfWork.HallSectorRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No HallSector to update");
            _mapper.Map(model, item);
            _unitOfWork.HallSectorRepository.Update(item);
            _unitOfWork.Save();
        }
    }
}
