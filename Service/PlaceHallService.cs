﻿using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceHall;

namespace Services.Service
{
    public interface IPlaceHallService
    {
        PlaceHall GetByID(long id);
        IEnumerable<PlaceHall> GetPlaceHalls();
        void Create(CreatePlaceHall model);
        void Update(long id, UpdatePlaceHall model);
        void Delete(long id);
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
        public void Create(CreatePlaceHall model)
        {
            var item = _mapper.Map<PlaceHall>(model);
            ValidateUniqueFields(item, "There is already existing same PlaceHallName for PlaceAddress");
            _unitOfWork.PlaceHallRepository.Insert(item);
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.PlaceHallRepository.Delete(id);
            _unitOfWork.Save();
        }

        public PlaceHall GetByID(long id)
        {
            return _unitOfWork.PlaceHallRepository.GetByID(id);
        }

        public IEnumerable<PlaceHall> GetPlaceHalls()
        {
            return _unitOfWork.PlaceHallRepository.Get();
        }

        public void Update(long id, UpdatePlaceHall model)
        {
            var item = _unitOfWork.PlaceHallRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No PlaceHall to update");
            _mapper.Map(model, item);
            ValidateUniqueFields(item, "There is already existing same PlaceHallName for PlaceAddress");
            _unitOfWork.PlaceHallRepository.Update(item);
            _unitOfWork.Save();
        }
        private void ValidateUniqueFields(PlaceHall model, string errorMessage)
        {
            if (_unitOfWork.PlaceHallRepository.Get(x => x.HallName == model.HallName && x.PlaceAddressID == x.PlaceAddressID).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
