using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceHall;
using EventSeller.Services.Interfaces;

namespace Services.Service
{
    /// <summary>
    /// Represents all actions with the <see cref="PlaceHall"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IPlaceHallService
    {
        /// <summary>
        /// Retrieves a place hall by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place hall.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the place hall.</returns>
        Task<PlaceHall> GetByID(long id);

        /// <summary>
        /// Retrieves a collection of all place halls.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of place halls.</returns>
        Task<IEnumerable<PlaceHall>> GetPlaceHalls();

        /// <summary>
        /// Creates a new place hall.
        /// </summary>
        /// <param name="model">The data transfer object containing place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddPlaceHallDto model);

        /// <summary>
        /// Updates an existing place hall.
        /// </summary>
        /// <param name="id">The identifier of the place hall to update.</param>
        /// <param name="model">The data transfer object containing updated place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditPlaceHallDto model);

        /// <summary>
        /// Deletes a place hall by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place hall to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }

    /// <summary>
    /// Represents the default implementation of the <see cref="IPlaceHallService"/>.
    /// </summary>
    /// <inheritdoc cref="IPlaceHallService"/>
    public class PlaceHallService : IPlaceHallService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceHallService"/> class with the specified unit of work and mapper.
        /// </summary>
        /// <param name="unitOfWork">The unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper.</param>
        public PlaceHallService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <inheritdoc/>
        public async Task Create(AddPlaceHallDto model)
        {
            var item = _mapper.Map<PlaceHall>(model);
            await ValidateUniqueFields(item, "There is already existing same PlaceHallName for PlaceAddress");
            await _unitOfWork.PlaceHallRepository.Insert(item);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public async Task Delete(long id)
        {
            await _unitOfWork.PlaceHallRepository.Delete(id);
            await _unitOfWork.Save();
        }
        /// <inheritdoc/>
        public Task<PlaceHall> GetByID(long id)
        {
            return _unitOfWork.PlaceHallRepository.GetByID(id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<PlaceHall>> GetPlaceHalls()
        {
            return _unitOfWork.PlaceHallRepository.Get();
        }
        /// <inheritdoc/>
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
        /// <summary>
        /// Validates that the place hall has unique fields before performing any database operations.
        /// </summary>
        /// <param name="model">The place hall model to validate.</param>
        /// <param name="errorMessage">The error message to throw if validation fails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a duplicate place hall is found.</exception>
        private async Task ValidateUniqueFields(PlaceHall model, string errorMessage)
        {
            if ((await _unitOfWork.PlaceHallRepository.Get(x => x.HallName == model.HallName && x.PlaceAddressID == x.PlaceAddressID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
    }
}
