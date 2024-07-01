using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.PlaceHall;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace Services.Service
{
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
        public async Task CreateAsync(AddPlaceHallDto model)
        {
            var item = _mapper.Map<PlaceHall>(model);
            await ValidateUniqueFieldsAsync(item, "There is already existing same PlaceHallName for PlaceAddress");
            await _unitOfWork.PlaceHallRepository.InsertAsync(item);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            await _unitOfWork.PlaceHallRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        /// <inheritdoc/>
        public Task<PlaceHall> GetByIDAsync(long id)
        {
            return _unitOfWork.PlaceHallRepository.GetByIDAsync(id);
        }
        /// <inheritdoc/>
        public async Task<bool> DoesExistsByIdAsync(long id)
        {
            return await _unitOfWork.PlaceHallRepository.DoesExistsAsync(obj => obj.ID==id);
        }
        /// <inheritdoc/>
        public Task<IEnumerable<PlaceHall>> GetPlaceHallsAsync()
        {
            return _unitOfWork.PlaceHallRepository.GetAsync();
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(long id, EditPlaceHallDto model)
        {
            var item = await _unitOfWork.PlaceHallRepository.GetByIDAsync(id);
            if (item == null)
                throw new NullReferenceException("No PlaceHall to update");
            _mapper.Map(model, item);
            await ValidateUniqueFieldsAsync(item, "There is already existing same PlaceHallName for PlaceAddress");
            _unitOfWork.PlaceHallRepository.Update(item);
            await _unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Validates that the place hall has unique fields before performing any database operations.
        /// </summary>
        /// <param name="model">The place hall model to validate.</param>
        /// <param name="errorMessage">The error message to throw if validation fails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a duplicate place hall is found.</exception>
        private async Task ValidateUniqueFieldsAsync(PlaceHall model, string errorMessage)
        {
            if ((await _unitOfWork.PlaceHallRepository.GetAsync(x => x.HallName == model.HallName && x.PlaceAddressID == x.PlaceAddressID)).Any())
                throw new InvalidOperationException(errorMessage);
        }
        public async Task<IEnumerable<TicketSeat>> GetAllSeatsInRangeByIdAsync(long placeHallId,int minRow, int maxRow) 
        {
            Expression<Func<TicketSeat, bool>> filter = ts => ts.HallSector.PlaceHallID == placeHallId && ts.PlaceRow >= minRow && ts.PlaceRow <= maxRow;
            return await _unitOfWork.TicketSeatRepository.GetAsync(filter,null, "HallSector");
        }
    }
}
