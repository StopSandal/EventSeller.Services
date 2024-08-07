﻿using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Ticket;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the service for ticket registration operations.
    /// </summary>
    public class TicketRegistrationService : ITicketRegistrationService
    {
        private const string EventIncludedProps = "EventType";

        private readonly ILogger<TicketRegistrationService> _logger;
        private readonly IPlaceHallService _placeHallService;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        private readonly IEventSessionService _eventSessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketRegistrationService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="placeHallService">The place hall service.</param>
        /// <param name="eventService">The event service.</param>
        /// <param name="ticketService">The ticket service.</param>
        /// <param name="eventSessionService">The event session service.</param>
        public TicketRegistrationService(
            ILogger<TicketRegistrationService> logger,
            IPlaceHallService placeHallService,
            IEventService eventService,
            ITicketService ticketService,
            IEventSessionService eventSessionService)
        {
            _logger = logger;
            _placeHallService = placeHallService;
            _eventService = eventService;
            _ticketService = ticketService;
            _eventSessionService = eventSessionService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync(AddTicketsForHallByCountDTO addTicketsForHallByCountDTO)
        {
            _logger.LogInformation("Starting AddTicketsForPlaceHallByCountAsync with {TicketsCount} tickets", addTicketsForHallByCountDTO.TicketsCount);

            if (addTicketsForHallByCountDTO.TicketsCount <= 0)
            {
                _logger.LogError("Ticket count cannot be lesser than 0");
                throw new InvalidOperationException("Ticket count cannot be lesser than 0");
            }

            var eventId = addTicketsForHallByCountDTO.addEventSessionDTO.EventID;
            if (!await _eventService.DoesExistsByIdAsync(eventId))
            {
                _logger.LogError("Event with ID {EventID} does not exist", eventId);
                throw new InvalidOperationException("Event doesn't exist");
            }

            if (!await _placeHallService.DoesExistsByIdAsync(addTicketsForHallByCountDTO.HallID))
            {
                _logger.LogError("Hall with ID {HallID} does not exist", addTicketsForHallByCountDTO.HallID);
                throw new InvalidOperationException("Hall doesn't exist");
            }

            var eventSession = await _eventSessionService.CreateAsync(addTicketsForHallByCountDTO.addEventSessionDTO);

            var name = addTicketsForHallByCountDTO.Name;
            var description = addTicketsForHallByCountDTO.Description;
            var price = addTicketsForHallByCountDTO.Price;
            var currencyType = addTicketsForHallByCountDTO.CurrencyType;
            var hallID = addTicketsForHallByCountDTO.HallID;

            var ticketList = Enumerable.Range(0, addTicketsForHallByCountDTO.TicketsCount)
                .Select(_ => new Ticket
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    CurrencyType = currencyType,
                    EventSessionID = eventSession.Id
                })
                .ToList();

            await _ticketService.AddTicketListAsync(ticketList);
            _logger.LogInformation("{TicketsCount} tickets added successfully for HallID {HallID} and EventID {EventID}", addTicketsForHallByCountDTO.TicketsCount, hallID, eventId);

            return ticketList;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync(AddTicketsForHallToFillDTO addTicketsForHallToFillDTO)
        {
            _logger.LogInformation("Starting AddTicketsForPlaceHallForAllSeatsAsync");

            var startRow = addTicketsForHallToFillDTO.SeatsStartRow;
            var endRow = addTicketsForHallToFillDTO.SeatsEndRow;

            if (startRow < 1 || endRow < 1 || startRow >= endRow)
            {
                _logger.LogError("Invalid start or end rows: startRow={StartRow}, endRow={EndRow}", startRow, endRow);
                throw new InvalidOperationException("Provided start or end rows are wrong");
            }

            var eventId = addTicketsForHallToFillDTO.addEventSessionDTO.EventID;

            var eventWithEventType = await _eventService.GetWithIncludesByIDAsync(eventId, EventIncludedProps);

            if (eventWithEventType == null || eventWithEventType.EventType == null)
            {
                _logger.LogError("Provided Event does not exist or is corrupted");
                throw new InvalidOperationException("Provided Event not exists or corrupted");
            }

            var minimumSeatRow = eventWithEventType.EventType.MinimalSeatRowForEvent;

            if (minimumSeatRow.HasValue && minimumSeatRow.Value > startRow)
            {
                _logger.LogError("Start row {StartRow} is less than the minimum seat row {MinimumSeatRow} for the event", startRow, minimumSeatRow.Value);
                throw new InvalidOperationException("For this event this seats row wouldn't exists");
            }

            var hallID = addTicketsForHallToFillDTO.HallID;

            if (!await _placeHallService.DoesExistsByIdAsync(hallID))
            {
                _logger.LogError("Hall with ID {HallID} does not exist", hallID);
                throw new InvalidOperationException("Hall doesn't exist");
            }

            var eventSession = await _eventSessionService.CreateAsync(addTicketsForHallToFillDTO.addEventSessionDTO);

            var seats = await _placeHallService.GetAllSeatsInRangeByIdAsync(hallID, startRow, endRow);

            if (!seats.Any())
            {
                _logger.LogError("No seats found in the specified range for HallID {HallID}", hallID);
                throw new InvalidOperationException("No seats found in the specified range");
            }

            var name = addTicketsForHallToFillDTO.Name;
            var description = addTicketsForHallToFillDTO.Description;
            var price = addTicketsForHallToFillDTO.Price;
            var currencyType = addTicketsForHallToFillDTO.CurrencyType;

            var ticketsList = seats.Select(seat => new Ticket
            {
                Name = name,
                Description = description,
                Price = price,
                CurrencyType = currencyType,
                SeatID = seat.ID,
                EventSessionID = eventSession.Id
            }).ToList();

            await _ticketService.AddTicketListAsync(ticketsList);
            _logger.LogInformation("{TicketsCount} tickets added successfully for HallID {HallID} and EventID {EventID}", ticketsList.Count, hallID, eventId);

            return ticketsList;
        }
    }
}