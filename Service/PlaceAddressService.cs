﻿using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceAddress;

namespace Services.Service
{
    public interface IPlaceAddressService
    {
        PlaceAddress GetByID(long id);
        IEnumerable<PlaceAddress> GetPlaceAddresss();
        void Create(CreatePlaceAddress model);
        void Update(long id, UpdatePlaceAddress model);
        void Delete(long id);
    }

    public class PlaceAddressService : IPlaceAddressService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        public PlaceAddressService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Create(CreatePlaceAddress model)
        {
            _unitOfWork.PlaceAddressRepository.Insert(_mapper.Map<PlaceAddress>(model));
            _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            _unitOfWork.PlaceAddressRepository.Delete(id);
            _unitOfWork.Save();
        }

        public PlaceAddress GetByID(long id)
        {
            return _unitOfWork.PlaceAddressRepository.GetByID(id);
        }

        public IEnumerable<PlaceAddress> GetPlaceAddresss()
        {
            return _unitOfWork.PlaceAddressRepository.Get();
        }

        public void Update(long id, UpdatePlaceAddress model)
        {
            var item = _unitOfWork.PlaceAddressRepository.GetByID(id);
            if (item == null)
                throw new NullReferenceException("No PlaceAddress to update");
            _mapper.Map(model, item);
            _unitOfWork.PlaceAddressRepository.Update(item);
            _unitOfWork.Save();
        }
    }
}