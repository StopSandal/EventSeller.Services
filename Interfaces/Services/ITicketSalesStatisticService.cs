using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Service interface for ticket sales statistics operations.
    /// </summary>
    public interface ITicketSalesStatisticService
    {
        /// <summary>
        /// Retrieves sales statistics for a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the event.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId);

        /// <summary>
        /// Retrieves sales statistics for a specific event session of an event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="eventSessionId">The ID of the event session.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the event session.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAndSessionAsync(long eventId, long eventSessionId);

        /// <summary>
        /// Retrieves aggregated sales statistics for multiple events.
        /// </summary>
        /// <param name="eventIds">The IDs of the events.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains aggregated sales statistics for the events.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventsAsync(IEnumerable<long> eventIds);

        /// <summary>
        /// Retrieves sales statistics for a specific event session.
        /// </summary>
        /// <param name="eventSessionId">The ID of the event session.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the event session.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId);

        /// <summary>
        /// Retrieves aggregated sales statistics for multiple event sessions.
        /// </summary>
        /// <param name="eventSessionIds">The IDs of the event sessions.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains aggregated sales statistics for the event sessions.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionsAsync(IEnumerable<long> eventSessionIds);

        /// <summary>
        /// Retrieves sales statistics for a specified period.
        /// </summary>
        /// <param name="firstPeriod">The start date of the period.</param>
        /// <param name="secondPeriod">The end date of the period.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the specified period.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod);

        /// <summary>
        /// Retrieves sales statistics for a specific week.
        /// </summary>
        /// <param name="weekStartDate">The start date of the week.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the week.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate);

        /// <summary>
        /// Retrieves sales statistics for a specific day.
        /// </summary>
        /// <param name="dayDate">The date of the day.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the day.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate);

        /// <summary>
        /// Retrieves sales statistics for a specific event type.
        /// </summary>
        /// <param name="eventTypeId">The ID of the event type.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains sales statistics for the event type.</returns>
        Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId);
    }
}
